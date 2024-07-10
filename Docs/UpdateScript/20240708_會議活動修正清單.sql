--會議資料修正
Select * From ActivityCategory Order By Id Desc


--1.新增資料表+資料(ActivityCategory_temp_old) 多一個對應欄位 map_temp_new_Id

--2.新增資料表+資料(ActivityCategory_temp_new)

--3.轉檔公式(FTISUserHistory 注意：會外Type=2和其他) 需整理沒對應的資料
Select Id, ActivityCategoryType From FTISUserHistory

--4.驗證(本機資料與正式站)
