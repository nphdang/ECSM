# Exceptional Contrast Set Mining (ECSM)
C# code of ECSM (AI 2016 conference)

# How to run
1. Download and install .NET Framework 4.0 (http://www.microsoft.com/en-us/download/details.aspx?id=17718).
2. Run the program. 
3. Click Browse to select a dataset and a dictionary (optional).
4. Specify the minimum difference support threshold (minDev).
5. Click Mining to generate exceptional contrast sets. Select Save results if you want to save found exceptional contrast sets into csv file.
6. Select Classification if you want to use exceptional contrast sets to classify test examples (we use 10-fold cross validation).
7. The result includes mining time (in seconds), # of exceptional contrast sets, and accuracy, F1-score, Kappa statistic for classification.

# Libraries
In the source code, we use ALGLIB (http://www.alglib.net/) to compute Chi-square distribution. We also use Accord.NET Framework (http://accord-framework.net/) to compute F1-score and Kappa statistic.

# Citation
If you use our source code, please cite our paper as follows:

Dang Nguyen, Wei Luo, Dinh Phung, Svetha Venkatesh (2016). Exceptional Contrast Set Mining: Moving Beyond the Deluge of the Obvious. AI 2016, Tasmania, Australia. Springer LNAI, 9992, 455-468
