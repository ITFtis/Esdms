--部門代碼轉換

Update FTISUserHistory Set DCode = '02' Where DCode = '1' And ActivityCategoryType = 1
Update FTISUserHistory Set DCode = '58' Where DCode = '2' And ActivityCategoryType = 1
Update FTISUserHistory Set DCode = '52' Where DCode = '3' And ActivityCategoryType = 1
Update FTISUserHistory Set DCode = '31' Where DCode = '4' And ActivityCategoryType = 1
Update FTISUserHistory Set DCode = '19' Where DCode = '5' And ActivityCategoryType = 1
Update FTISUserHistory Set DCode = '16' Where DCode = '6' And ActivityCategoryType = 1
Update FTISUserHistory Set DCode = '22' Where DCode = '7' And ActivityCategoryType = 1
Update FTISUserHistory Set DCode = '17' Where DCode = '8' And ActivityCategoryType = 1

--
Alter TABLE FTISUserHistory ALTER COLUMN [DCode] [nvarchar](50) NULL
