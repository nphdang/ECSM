using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using Accord.Statistics.Analysis;

namespace ECSM
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        // # nodes on lattice
        int nNode = 0;
        // redundant csets
        int nIdenticalSup = 0, nDecreasingSig = 0, nIndependence = 0;
        // set of large 1-itemset candidates
        List<Node> LargeCandidates_1 = new List<Node>();
        // set of large k-itemset candidates
        List<Node> LargeCandidates_k = new List<Node>();
        // set of csets using FDR correction
        List<CSet> CSs = new List<CSet>();
        // lattice
        Dictionary<int, Node> Lattice = new Dictionary<int, Node>();
        // default class for test case
        int DEFAULT_CLASS = 0;

        #region buttons
        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btBrowseData_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtDatabase.Text = openFileDialog1.FileName;
            }
        }

        private void txtBrowseDict_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                txtDict.Text = openFileDialog2.FileName;
            }
        }

        private void chkSignificant_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSignificant.Checked == false)
            {
                chkExceptional.Checked = false;
                chkExceptional.Enabled = false;
            }
            else
            {
                chkExceptional.Enabled = true;
            }
        } 
        #endregion

        #region read data
        Dataset readData(string _sFile)
        {
            Dataset dt = null;
            string[] line = null;
            using (StreamReader sr = File.OpenText(_sFile))
            {
                line = sr.ReadLine().Split(',');
                int row = int.Parse(line[0]);
                int col = int.Parse(line[1]);
                ushort cls = ushort.Parse(line[2]);
                dt = new Dataset(row, col);
                dt.cls = cls;
                dt.row_cls = new int[dt.cls];
                for (int i = 0; i < dt.row; i++)
                {
                    line = sr.ReadLine().Split(',');
                    for (int j = 0; j < dt.col; j++)
                    {
                        dt.data[i][j] = int.Parse(line[j]);
                    }
                    for (ushort x = 0; x < cls; x++)
                    {
                        if (x == dt.data[i][dt.col - 1])
                        {
                            dt.row_cls[x]++;
                        }
                    }
                }                
            }    
        
            return dt;
        }
        #endregion       

        #region read dictionary
        Dictionary<int, string[]> readDict(string _sFile)
        {
            Dictionary<int, string[]> dict = new Dictionary<int, string[]>();
            using (StreamReader sr = File.OpenText(_sFile))
            {
                string line = "";
                int nAtt = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] values = line.Split(',');
                    dict.Add(nAtt++, values);
                }
            }

            return dict;
        }
        #endregion

        #region intersect of two Obidsets
        List<int> intersectObidsets(List<int> a1, List<int> a2)
        {
            List<int> a = new List<int>();
            if (a1.Count == 0 || a2.Count == 0)
            {
                return a;
            }

            int i = 0, j = 0;
            a2.Add(a1[a1.Count - 1] + 1);
            while (i < a1.Count)
            {
                if (a1[i] < a2[j])
                {
                    i++;
                }
                else if (a1[i] > a2[j])
                {
                    j++;
                }
                else
                {
                    a.Add(a1[i++]);
                    j++;
                }
            }
            a2.RemoveAt(a2.Count - 1);

            return a;
        }
        #endregion
       
        #region union of two itemsets
        List<Item> unionItemsets(List<Item> a1, List<Item> a2)
        {
            List<Item> a = new List<Item>();
            for (int i = 0; i < a1.Count; i++)
            {
                a.Add(a1[i]);
            }
            a.Add(a2[a2.Count - 1]);

            return a;
        }
        #endregion
                
        #region find candidates that contain 1-itemsets
        void findCandidates(Dataset dt, double minDev)
        {
            // array contains items
            Node[] items = new Node[dt.row];
            // real # items
            int nN = 0;
            int k = 0;

            // for each attribute: i
            for (int i = 0; i < dt.col - 1; i++)
            {
                nN = 0;
                // for each transaction: j
                for (int j = 0; j < dt.row; j++)
                {
                    // check old items
                    for (k = 0; k < nN; k++)
                    {
                        if (items[k].itemset[0].val == dt.data[j][i])
                        {
                            // find Obidset_i
                            items[k].Obidset[dt.data[j][dt.col - 1]].Add(j);
                            break;
                        }
                    }
                    // new item
                    if (k == nN)
                    {
                        Item item = new Item();
                        item.att = i;
                        item.val = dt.data[j][i];
                        items[nN] = new Node(dt.cls);
                        items[nN].itemset.Add(item);
                        // find Obidset_i
                        items[nN].Obidset[dt.data[j][dt.col - 1]].Add(j);
                        nN++;
                    }
                }

                for (k = 0; k < nN; k++)
                {
                    Node node = items[k];
                    // compute dev
                    double min_pro = (double)node.Obidset[0].Count / dt.row_cls[0];
                    double max_pro = (double)node.Obidset[0].Count / dt.row_cls[0];
                    for (ushort x = 0; x < dt.cls; x++)
                    {
                        double pro = (double)node.Obidset[x].Count / dt.row_cls[x];
                        if (pro > max_pro)
                        {
                            max_pro = pro;
                            node.max_group = x;
                        }
                        if (pro < min_pro)
                        {
                            min_pro = pro;
                        }
                        // compute pos
                        if (node.Obidset[x].Count() > node.Obidset[node.pos].Count())
                        {
                            node.pos = x;
                        }
                        // compute total   
                        node.total += node.Obidset[x].Count;
                    }
                    // check if this node is large
                    if (max_pro >= minDev)
                    {
                        node.id = nNode;
                        node.itemset[0].id = nNode;
                        // increase # nodes on lattice
                        nNode++;
                        node.max_pro = max_pro;
                        // compute support diff
                        node.dev = max_pro - min_pro;
                        // convert att to bit representation                    
                        node.att = (long)Math.Pow(2, i);
                        Lattice.Add(node.id, node);
                    }
                }
            }
        }
        #endregion                      
                       
        #region CREATE-LATTICE fuction
        void CREATE_LATTICE(List<Node> Lr, double minDev, int row, ushort cls, int[] row_cls)
        {
            int _iLr_Count = Lr.Count;
            for (int i = 0; i < _iLr_Count; i++)
            {
                Node li = (Node)Lr[i];

                #region redundant nodes
                // not interesting: this node has support for only one group (jumping EPs)
                if (li.Obidset[li.pos].Count() == li.total)
                {
                    nIdenticalSup++;
                    continue;
                }                
                // not interesting: prune nodes with small cell frequencies
                bool large_cell = true;
                for (ushort x = 0; x < cls; x++)
                {
                    double exp_val1 = (double)li.total * row_cls[x] / row;
                    double exp_val2 = (double)(row - li.total) * row_cls[x] / row;
                    // all cell frequencies are less than 1
                    if (exp_val1 < 1 || exp_val2 < 1)
                    {
                        large_cell = false;
                        break;
                    }
                }
                if (large_cell == false)
                {
                    continue;
                }
                #endregion

                List<Node> P_i = new List<Node>();
                for (int j = i + 1; j < _iLr_Count; j++)
                {
                    Node lj = (Node)Lr[j];
                    if (li.att != lj.att)
                    {
                        Node O = new Node(cls);
                        // compute dev
                        O.Obidset[0] = intersectObidsets(li.Obidset[0], lj.Obidset[0]);
                        O.total += O.Obidset[0].Count;
                        double min_pro = (double)O.Obidset[0].Count / row_cls[0];
                        double max_pro = (double)O.Obidset[0].Count / row_cls[0];
                        for (ushort x = 1; x < cls; x++)
                        {
                            O.Obidset[x] = intersectObidsets(li.Obidset[x], lj.Obidset[x]);
                            double pro = (double)O.Obidset[x].Count / row_cls[x];
                            if (pro > max_pro)
                            {
                                max_pro = pro;
                                O.max_group = x; // dominant group
                            }
                            if (pro < min_pro)
                            {
                                min_pro = pro;
                            }
                            if (O.Obidset[x].Count() > O.Obidset[O.pos].Count())
                            {
                                O.pos = x;
                            }
                            // compute total
                            O.total += O.Obidset[x].Count;
                        }
                        // check if this node is large
                        if (max_pro >= minDev)
                        {
                            O.id = nNode;
                            nNode++;
                            O.max_pro = max_pro;
                            // compute support diff
                            O.dev = max_pro - min_pro;
                            O.att = li.att | lj.att;
                            O.itemset = unionItemsets(li.itemset, lj.itemset);
                            // update lattice
                            O.pEC.Add(li.id);
                            li.cEC.Add(O.id);
                            O.pL.Add(lj.id);
                            lj.cL.Add(O.id);
                            UPDATE_LATTICE(li, O);
                            P_i.Add(O);
                            Lattice.Add(O.id, O);
                        }                                                                      
                    }
                }
                if (P_i.Count() > 0)
                {
                    CREATE_LATTICE(P_i, minDev, row, cls, row_cls);
                }
            }
        }
        #endregion

        #region UPDATE-LATTICE function
        void UPDATE_LATTICE(Node parent, Node X)
        {
            foreach (int li in parent.cL)
            {
                Node node_li = (Node)Lattice[li];
                foreach (int lj in node_li.cEC)
                {
                    Node node_lj = (Node)Lattice[lj];
                    long att = node_lj.att & X.att;
                    if (att == X.att)
                    {
                        if (X.itemset[X.itemset.Count - 1].val == node_lj.itemset[node_lj.itemset.Count - 1].val)
                        {
                            node_lj.pL.Add(X.id);
                            X.cL.Add(node_lj.id);
                        }
                    }
                }
            }
        }
        #endregion                

        #region FIND-LARGE-CS function
        void FIND_LARGE_CS(Node node, double minDev, int row, ushort cls, int[] row_cls)
        {
            // cs is large
            if (node.dev >= minDev)
            {
                // compute Degree of Freedom = (r-1)x(c-1)
                int df = cls - 1;
                // compute Critical Value (CV)
                double cv1 = 0;
                double cv2 = 0;
                for (ushort x = 0; x < cls; x++)
                {
                    double exp1 = (double)node.total * row_cls[x] / row;
                    cv1 += Math.Pow(node.Obidset[x].Count - exp1, 2) / exp1;
                    double exp2 = (double)(row - node.total) * row_cls[x] / row;
                    cv2 += Math.Pow(row_cls[x] - node.Obidset[x].Count - exp2, 2) / exp2;
                }
                double cv = cv1 + cv2;
                // compute chi-square
                node.chi = cv;
                // compute p-value
                node.p_value = 1 - alglib.chisquaredistribution(df, cv);

                #region identify redundant nodes
                List<int> parent = node.pEC.Concat(node.pL).ToList();
                // interesting: cs doesn't have the same support with its parent
                bool same_par_sup = false;                                               
                foreach (int id in parent)
                {
                    Node node_p = Lattice[id];
                    if (node.total == node_p.total)
                    {                        
                        same_par_sup = true;
                        break;
                    }
                }
                if (same_par_sup)
                {
                    nIdenticalSup++;
                }
                // interesting: cs is statistical surprise
                bool stat_surprise = false;
                for (ushort x = 0; x < cls; x++)
                {
                    double pro_exp = 1;
                    foreach (int id in parent)
                    {
                        Node node_p = Lattice[id];
                        pro_exp *= (double)node_p.Obidset[x].Count / row_cls[x];
                    }                                                     
                    double pro = (double)node.Obidset[x].Count / row_cls[x];
                    if (Math.Abs(pro_exp - pro) >= 0.01)
                    {                        
                        stat_surprise = true;
                        break;
                    }                    
                }
                if (stat_surprise == false)
                {
                    nIndependence++;
                }
                
                if (!same_par_sup && stat_surprise)
                {
                    if (chkPruneChi.Checked)
                    {
                        // interesting: cs is chi-square surprise
                        bool chi_surprise = true;
                        foreach (int id in parent)
                        {
                            Node node_p = Lattice[id];
                            if (node.chi < node_p.chi)
                            {
                                chi_surprise = false;
                                break;
                            }
                        }
                        if (chi_surprise)
                        {
                            if (node.itemset.Count == 1)
                            {
                                LargeCandidates_1.Add(node); // this candidate of size 1 is large
                            }
                            else if (node.itemset.Count > 1)
                            {
                                LargeCandidates_k.Add(node); // this candidate of size k>1 is large
                            }
                        }     
                        else
                        {
                            nDecreasingSig++;
                        }
                    }
                    else
                    {
                        if (node.itemset.Count == 1)
                        {
                            LargeCandidates_1.Add(node); // this candidate of size 1 is large
                        }
                        else if (node.itemset.Count > 1)
                        {
                            LargeCandidates_k.Add(node); // this candidate of size k>1 is large
                        }
                    }
                }
                #endregion
            }
        }
        #endregion

        #region write result of contrast sets
        void writeCS(String file, Dictionary<int, string[]> dict, List<CSet> Result, int cls)
        {
            using (StreamWriter sw = new StreamWriter(file))
            {
                string col_name = "G_ID,Len,Cov,Diff_Supp,Chi_Square,Contrast_Pattern,";
                for (ushort x = 0; x < cls; x++)
                {
                    col_name += dict[dict.Count - 2][x] + ",";
                }
                col_name = col_name.Substring(0, col_name.Length - 1);
                sw.WriteLine(col_name);
                foreach (CSet cs in Result)
                {
                    string itemset = "";
                    string proportion = "";
                    foreach (Item item in cs.C_itemset)
                    {
                        string att = dict[dict.Count - 1][item.att];
                        string val = dict[item.att][item.val - 1];
                        itemset += att + "=" + val + " & ";
                    }
                    for (ushort x = 0; x < cls; x++)
                    {
                        double pro = cs.Pro[x];
                        proportion += Math.Round(pro * 100, 2) + "%,";
                    }
                    proportion = proportion.Substring(0, proportion.Length - 1);
                    itemset = itemset.Substring(0, itemset.Length - 3);
                    sw.WriteLine(cs.g_id.ToString() + "," + cs.C_itemset.Count + "," + Math.Round(cs.Sup * 100, 2) + "," + 
                                    Math.Round(cs.Dev * 100, 2) + "," + Math.Round(cs.Chi, 2) + "," + itemset + "," + proportion);
                }
            }
        }
        #endregion                     

        #region check whether a cset covers a test case
        bool checkRuleCover(CSet cs, int[] test_case)
        {
            foreach(Item item in cs.C_itemset)
            {
                if (item.val != test_case[item.att])
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region classify a test case
        // use affirmative and exceptional csets to predict
        int assignClass(List<CSet> CSs, int[] test_case, ushort cls, int[] row_cls)
        {
            double[] score = new double[cls];
            foreach (CSet cs in CSs)
            {
                if (checkRuleCover(cs, test_case))
                {
                    for (ushort x = 0; x < cls; x++)
                    {
                        score[x] += cs.Pro[x];                        
                    }
                }
            }
            int predict_class = -1;
            if (score.Max() == 0) // no rule covers
            {
                DEFAULT_CLASS++;
                // assign the majority class
                predict_class = Array.IndexOf(row_cls, row_cls.Max());
            }
            else
            {
                // get max value and index in score    
                predict_class = Array.IndexOf(score, score.Max());
            }           
            
            return predict_class;
        }        
        #endregion

        private void btMining_Click(object sender, EventArgs e)
        {            
            string _sFileData = txtDatabase.Text.Trim();
            double alpha = double.Parse(txtAlpha.Text.Trim());
            double minDev = double.Parse(txtMinDev.Text.Trim()) / 100;
            TreeNode tnode = treeResult.Nodes.Add("alpha = " + txtAlpha.Text + ", minDev = " + txtMinDev.Text + "%");
            Dataset dt = readData(_sFileData);
            // 10-fold validation
            int tr_row = (int) Math.Round(dt.row * 0.9);            
            int te_row = dt.row - tr_row;
            Stopwatch sw = Stopwatch.StartNew();
            if (chkClassification.Checked) // classification
            {
                int n_run = 10; // use n_run=1 if only mine exceptional csets
                double[] acc = new double[n_run];
                double[] fsc = new double[n_run];
                double[] kap = new double[n_run];
                for (int run = 0; run < n_run; run++)
                {
                    // reset values
                    nNode = 0;
                    nIdenticalSup = 0; nDecreasingSig = 0; nIndependence = 0;
                    LargeCandidates_1 = new List<Node>();
                    LargeCandidates_k = new List<Node>();
                    CSs = new List<CSet>();
                    Lattice = new Dictionary<int, Node>();
                    DEFAULT_CLASS = 0;

                    #region generate training and test
                    Dataset train = new Dataset(tr_row, dt.col);
                    train.cls = dt.cls;
                    train.row_cls = new int[train.cls];
                    Dataset test = new Dataset(te_row, dt.col);
                    test.cls = dt.cls;
                    test.row_cls = new int[test.cls];
                    Random rnd = new Random(run);
                    int[] train_idx = Enumerable.Range(0, dt.row).OrderBy(x => rnd.Next()).Take(train.row).ToArray();
                    int tr_id = 0; int te_id = 0;
                    for (int i = 0; i < dt.row; i++)
                    {
                        if (train_idx.Contains(i))
                        {
                            train.data[tr_id] = dt.data[i];
                            tr_id++;
                        }
                        else
                        {
                            test.data[te_id] = dt.data[i];
                            te_id++;
                        }
                    }
                    // get # transactions in each class
                    for (int i = 0; i < train.row; i++)
                    {
                        for (ushort x = 0; x < train.cls; x++)
                        {
                            if (x == train.data[i][train.col - 1])
                            {
                                train.row_cls[x]++;
                            }
                        }
                    }
                    for (int i = 0; i < test.row; i++)
                    {
                        for (ushort x = 0; x < test.cls; x++)
                        {
                            if (x == test.data[i][test.col - 1])
                            {
                                test.row_cls[x]++;
                            }
                        }
                    }
                    #endregion

                    // use dt if only mine exceptional csets
                    // use train for classification
                    Dataset dt_cs = train;                     
                    // obtain candidates that containt 1-itemsets
                    findCandidates(dt_cs, minDev);
                    List<Node> Lr = Lattice.Values.ToList();
                    // create lattice
                    CREATE_LATTICE(Lr, minDev, dt_cs.row, dt_cs.cls, dt_cs.row_cls);
                    // find large and non-redundant csets
                    foreach (Node node in Lattice.Values)
                    {
                        FIND_LARGE_CS(node, minDev, dt_cs.row, dt_cs.cls, dt_cs.row_cls);
                    }
                    int nLarge = LargeCandidates_1.Count + LargeCandidates_k.Count;

                    #region generate significant csets using FDR method
                    if (chkSignificant.Checked)
                    {
                        // contrast sets of size 1
                        List<int> CSs_1 = new List<int>();
                        foreach (Node node in LargeCandidates_1)
                        {
                            if (node.p_value <= alpha)
                            {
                                CSet cs = new CSet(dt_cs.cls)
                                {
                                    g_id = node.id,
                                    C_itemset = node.itemset,
                                    Sup = (double)node.total / dt_cs.row,
                                    Dev = node.dev,
                                    Chi = node.chi
                                };

                                for (ushort x = 0; x < dt_cs.cls; x++)
                                {
                                    double pro = (double)node.Obidset[x].Count / dt_cs.row_cls[x];
                                    cs.Pro[x] = pro;
                                }
                                CSs.Add(cs);
                                CSs_1.Add(node.id);
                            }
                        }
                        // contrast sets of size k>1
                        int nLarge_k = LargeCandidates_k.Count;
                        // sort large candidates in ascending order of p-value: very faster than use code
                        List<Node> sortedLargeCandidates_k = LargeCandidates_k.OrderBy(x => x.p_value).ToList();
                        int kk = 0;
                        for (kk = 0; kk < nLarge_k; kk++)
                        {
                            Node node = sortedLargeCandidates_k[kk];
                            double fdr = (double)(kk + 1) * alpha / nLarge_k;
                            if (node.p_value > fdr)
                            {
                                break;
                            }
                        }
                        for (int j = 0; j < kk; j++)
                        {
                            Node node = sortedLargeCandidates_k[j];
                            if (chkExceptional.Checked) // exceptional contrast sets
                            {
                                // this id is not the node id on lattice
                                // it is used for group id
                                int g_node_id = -1;
                                // check dominant group of this node whether it is different than its parent of size 1
                                foreach (Item item in node.itemset)
                                {
                                    if (CSs_1.Contains(item.id))
                                    {
                                        Node node_it = Lattice[item.id];
                                        if (node.max_group != node_it.max_group)
                                        {
                                            g_node_id = item.id;
                                        }
                                        else
                                        {
                                            if (node.chi < node_it.chi)
                                            {
                                                g_node_id = -1;
                                                break;
                                            }
                                        }
                                    }                                    
                                }
                                if (g_node_id != -1)
                                {
                                    CSet cs = new CSet(dt_cs.cls)
                                    {
                                        g_id = g_node_id,
                                        C_itemset = node.itemset,
                                        Sup = (double)node.total / dt_cs.row,
                                        Dev = node.dev,
                                        Chi = node.chi
                                    };
                                    for (ushort x = 0; x < dt_cs.cls; x++)
                                    {
                                        double pro = (double)node.Obidset[x].Count / dt_cs.row_cls[x];
                                        cs.Pro[x] = pro;
                                    }
                                    CSs.Add(cs);
                                }
                            }
                            else // only significant contrast sets
                            {
                                CSet cs = new CSet(dt_cs.cls)
                                {
                                    g_id = node.id,
                                    C_itemset = node.itemset,
                                    Sup = (double)node.total / dt_cs.row,
                                    Dev = node.dev,
                                    Chi = node.chi
                                };
                                for (ushort x = 0; x < dt_cs.cls; x++)
                                {
                                    double pro = (double)node.Obidset[x].Count / dt_cs.row_cls[x];
                                    cs.Pro[x] = pro;
                                }
                                CSs.Add(cs);
                            }
                        }
                    }
                    else // only large contrast sets
                    {
                        // contrast sets of size 1
                        foreach (Node node in LargeCandidates_1)
                        {
                            CSet cs = new CSet(dt_cs.cls)
                            {
                                C_itemset = node.itemset,
                                Sup = (double)node.total / dt_cs.row,
                                Dev = node.dev,
                                Chi = node.chi
                            };

                            for (ushort x = 0; x < dt_cs.cls; x++)
                            {
                                double pro = (double)node.Obidset[x].Count / dt_cs.row_cls[x];
                                cs.Pro[x] = pro;
                            }
                            CSs.Add(cs);
                        }
                        // contrast sets of size k>1
                        foreach (Node node in LargeCandidates_k)
                        {
                            CSet cs = new CSet(dt_cs.cls)
                            {
                                C_itemset = node.itemset,
                                Sup = (double)node.total / dt_cs.row,
                                Dev = node.dev,
                                Chi = node.chi
                            };

                            for (ushort x = 0; x < dt_cs.cls; x++)
                            {
                                double pro = (double)node.Obidset[x].Count / dt_cs.row_cls[x];
                                cs.Pro[x] = pro;
                            }
                            CSs.Add(cs);
                        }
                    }
                    #endregion

                    #region classification
                    int[] t_class = new int[test.row]; // true class
                    int[] p_class = new int[test.row]; // predicted class
                    for (int i = 0; i < test.row; i++)
                    {
                        int[] test_case = test.data[i];
                        int true_class = test_case[test.col - 1];
                        int predict_class = assignClass(CSs, test_case, test.cls, test.row_cls);
                        t_class[i] = true_class;
                        p_class[i] = predict_class;
                        if (predict_class == true_class)
                        {
                            acc[run] += 1;
                        }
                    }
                    acc[run] = Math.Round(acc[run] / test.row, 4);
                    ConfusionMatrix cm = new ConfusionMatrix(t_class, p_class);
                    fsc[run] = Math.Round(cm.FScore, 4);
                    kap[run] = Math.Round(cm.Kappa, 4);
                    #endregion                    
                }
                sw.Stop();
                long timeMining = sw.ElapsedMilliseconds;
                tnode.Nodes.Add("Training time: " + timeMining / 1000.0 + " (s)");
                tnode.Nodes.Add("Default class: " + DEFAULT_CLASS + ". Accuracy: " + acc.Sum() / n_run +
                                ". F1-score: " + fsc.Sum() / n_run + ". Kappa: " + kap.Sum() / n_run);
            }
            else // only mine exceptional csets
            {
                // reset values
                nNode = 0;
                nIdenticalSup = 0; nDecreasingSig = 0; nIndependence = 0;
                LargeCandidates_1 = new List<Node>();
                LargeCandidates_k = new List<Node>();
                CSs = new List<CSet>();
                Lattice = new Dictionary<int, Node>();
                DEFAULT_CLASS = 0;

                // use dt if only mine exceptional csets
                // use train for classification
                Dataset dt_cs = dt;
                // obtain candidates that containt 1-itemsets
                findCandidates(dt_cs, minDev);
                List<Node> Lr = Lattice.Values.ToList();
                // create lattice
                CREATE_LATTICE(Lr, minDev, dt_cs.row, dt_cs.cls, dt_cs.row_cls);
                // find large and non-redundant csets
                foreach (Node node in Lattice.Values)
                {
                    FIND_LARGE_CS(node, minDev, dt_cs.row, dt_cs.cls, dt_cs.row_cls);
                }
                int nLarge = LargeCandidates_1.Count + LargeCandidates_k.Count;

                #region generate significant csets using FDR method
                if (chkSignificant.Checked)
                {
                    // contrast sets of size 1
                    List<int> CSs_1 = new List<int>();
                    foreach (Node node in LargeCandidates_1)
                    {
                        if (node.p_value <= alpha)
                        {
                            CSet cs = new CSet(dt_cs.cls)
                            {
                                g_id = node.id,
                                C_itemset = node.itemset,
                                Sup = (double)node.total / dt_cs.row,
                                Dev = node.dev,
                                Chi = node.chi
                            };

                            for (ushort x = 0; x < dt_cs.cls; x++)
                            {
                                double pro = (double)node.Obidset[x].Count / dt_cs.row_cls[x];
                                cs.Pro[x] = pro;
                            }
                            CSs.Add(cs);
                            CSs_1.Add(node.id);
                        }
                    }
                    // contrast sets of size k>1
                    int nLarge_k = LargeCandidates_k.Count;
                    // sort large candidates in ascending order of p-value: very faster than use code
                    List<Node> sortedLargeCandidates_k = LargeCandidates_k.OrderBy(x => x.p_value).ToList();
                    int kk = 0;
                    for (kk = 0; kk < nLarge_k; kk++)
                    {
                        Node node = sortedLargeCandidates_k[kk];
                        double fdr = (double)(kk + 1) * alpha / nLarge_k;
                        if (node.p_value > fdr)
                        {
                            break;
                        }
                    }
                    for (int j = 0; j < kk; j++)
                    {
                        Node node = sortedLargeCandidates_k[j];
                        if (chkExceptional.Checked) // exceptional contrast sets
                        {
                            // this id is not the node id on lattice
                            // it is used for group id
                            int g_node_id = -1;
                            // check dominant group of this node whether it is different than its parent of size 1
                            foreach (Item item in node.itemset)
                            {
                                if (CSs_1.Contains(item.id))
                                {
                                    Node node_it = Lattice[item.id];
                                    if (node.max_group != node_it.max_group)
                                    {
                                        g_node_id = item.id;
                                    }
                                    else
                                    {
                                        if (node.chi < node_it.chi)
                                        {
                                            g_node_id = -1;
                                            break;
                                        }
                                    }
                                }                                
                            }
                            if (g_node_id != -1)
                            {
                                CSet cs = new CSet(dt_cs.cls)
                                {
                                    g_id = g_node_id,
                                    C_itemset = node.itemset,
                                    Sup = (double)node.total / dt_cs.row,
                                    Dev = node.dev,
                                    Chi = node.chi
                                };
                                for (ushort x = 0; x < dt_cs.cls; x++)
                                {
                                    double pro = (double)node.Obidset[x].Count / dt_cs.row_cls[x];
                                    cs.Pro[x] = pro;
                                }
                                CSs.Add(cs);
                            }
                        }
                        else // only significant contrast sets
                        {
                            CSet cs = new CSet(dt_cs.cls)
                            {
                                g_id = node.id,
                                C_itemset = node.itemset,
                                Sup = (double)node.total / dt_cs.row,
                                Dev = node.dev,
                                Chi = node.chi
                            };
                            for (ushort x = 0; x < dt_cs.cls; x++)
                            {
                                double pro = (double)node.Obidset[x].Count / dt_cs.row_cls[x];
                                cs.Pro[x] = pro;
                            }
                            CSs.Add(cs);
                        }
                    }
                }
                else // only large contrast sets
                {
                    // contrast sets of size 1
                    foreach (Node node in LargeCandidates_1)
                    {
                        CSet cs = new CSet(dt_cs.cls)
                        {
                            C_itemset = node.itemset,
                            Sup = (double)node.total / dt_cs.row,
                            Dev = node.dev,
                            Chi = node.chi
                        };

                        for (ushort x = 0; x < dt_cs.cls; x++)
                        {
                            double pro = (double)node.Obidset[x].Count / dt_cs.row_cls[x];
                            cs.Pro[x] = pro;
                        }
                        CSs.Add(cs);
                    }
                    // contrast sets of size k>1
                    foreach (Node node in LargeCandidates_k)
                    {
                        CSet cs = new CSet(dt_cs.cls)
                        {
                            C_itemset = node.itemset,
                            Sup = (double)node.total / dt_cs.row,
                            Dev = node.dev,
                            Chi = node.chi
                        };

                        for (ushort x = 0; x < dt_cs.cls; x++)
                        {
                            double pro = (double)node.Obidset[x].Count / dt_cs.row_cls[x];
                            cs.Pro[x] = pro;
                        }
                        CSs.Add(cs);
                    }
                }
                #endregion

                sw.Stop();
                long timeMining = sw.ElapsedMilliseconds;                

                #region print results
                tnode.Nodes.Add("Training time: " + timeMining / 1000.0 + " (s)");
                tnode.Nodes.Add("Identical: " + nIdenticalSup + ". Decrease: " + nDecreasingSig + ". Independence: " + nIndependence);
                if (chkSignificant.Checked)
                {
                    if (chkExceptional.Checked)
                    {
                        tnode.Nodes.Add("Large: " + nLarge + ". Exceptional: " + CSs.Count());
                    }
                    else
                    {
                        tnode.Nodes.Add("Large: " + nLarge + ". Significant: " + CSs.Count());
                    }
                }
                else // large csets
                {
                    tnode.Nodes.Add("Large: " + nLarge);
                }

                if (chkOutput.Checked)
                {
                    string _sFileDict = txtDict.Text.Trim();
                    Dictionary<int, string[]> dict = readDict(_sFileDict);
                    if (CSs.Count > 0)
                    {
                        // sort contrast sets based on group id
                        List<CSet> sortedCSs = CSs.OrderBy(x => x.g_id).ToList();
                        string file_path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        if (chkSignificant.Checked)
                        {
                            if (chkExceptional.Checked)
                            {
                                file_path += @"\ECSM_Exceptional (" + alpha + ", " + txtMinDev.Text + "%)";
                            }
                            else
                            {
                                file_path += @"\ECSM_Significant (" + alpha + ", " + txtMinDev.Text + "%)";
                            }
                        }
                        else
                        {
                            file_path += @"\ECSM_Large (" + txtMinDev.Text + "%)";
                        }
                        writeCS(file_path + ".csv", dict, sortedCSs, dt_cs.cls);
                    }
                    MessageBox.Show("Finish!");
                }
                #endregion                
            }
        }        
    }
}
