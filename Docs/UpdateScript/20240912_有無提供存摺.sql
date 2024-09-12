--有無提供存摺
alter Table BasicUser add HasPassbook [nvarchar](1) NULL
Go

Update BasicUser Set HasPassbook = 'N'

