create database QuanLyQuanCafe
go

use QuanLyQuanCafe
go

-- Beverage
-- Table
-- BeverageCategory
-- Account
-- Bill
-- BillInfo

create table TableBeverage (
	T_id int identity primary key,
	T_name nvarchar(100) not null,
	T_status nvarchar(100) default N'Trống' not null -- Trống || Có người
)
go

create table Account (
	A_userName nvarchar(100) primary key not null,
	A_displayName nvarchar(100) not null,
	A_passWord nvarchar(1000) not null,
	A_role int not null default 0 -- 1: Admin || 0: Staff
)
go

create table BeverageCategory(
	BC_id int identity primary key,
	BC_name nvarchar(100) not null
)
go

create table Beverage(
	BE_id int identity primary key,
	BE_name nvarchar(100) not null,
	BE_idCategory int not null,
	BE_price float not null default 0

	foreign key (BE_idCategory) references dbo.BeverageCategory(BC_id)
)
go

create table Bill(
	B_id int identity primary key,
	B_dateCheckIn date not null default getdate(),
	B_dateCheckOut date,
	B_idTable int not null,
	B_status int not null default 0 -- 1: Đã thanh toán || 0: Chưa thanh toán

	foreign key (B_idTable) references dbo.TableBeverage(T_id)
)
go

create table BillInfo(
	BI_id int identity primary key,
	BI_idBill int not null,
	BI_idBeverage int not null,
	BI_count int not null default 0 -- Số lượng món

	foreign key (BI_idBill) references dbo.Bill(B_id),
	foreign key (BI_idBeverage) references dbo.Beverage(BE_id)
)
go

-- Alter database
alter table dbo.Bill add B_discount int;
go
ALTER TABLE Account
ADD CONSTRAINT DF_A_passWord DEFAULT '1' FOR A_passWord;
Go

update dbo.Bill set B_discount = 0;

-- Insert database --
-- Account
insert into dbo.Account(A_userName, A_displayName, A_passWord, A_role) values (N'admin', N'Admin', N'1', 1);
insert into dbo.Account(A_userName, A_displayName, A_passWord, A_role) values (N'tai', N'TanTai', N'1', 0);

select * from dbo.account;
select A_userName as [Tên tài khoản], A_displayName as [Tên hiển thị], A_role as [Vai trò] from dbo.account
go

select * from dbo.Account where A_userName = N'admin' and A_passWord = N'1';
go

-- Insert TableBeverage
insert into dbo.TableBeverage(T_name, T_status) values (N'Bàn 1', N'Trống');
insert into dbo.TableBeverage(T_name, T_status) values (N'Bàn 2', N'Có người');

-- Fuction insert table
declare @i int = 0
while @i <= 10
begin
	insert dbo.TableBeverage (T_name) values (N'Bàn' + cast(@i as nvarchar(100)))
	set @i = @i + 1
end
go

-- Bill
insert into dbo.Bill(B_dateCheckIn, B_dateCheckOut, B_idTable, B_status) values (GETDATE(), null, 5, 0);
insert into dbo.Bill(B_dateCheckIn, B_dateCheckOut, B_idTable, B_status) values (GETDATE(), null, 6, 0);
insert into dbo.Bill(B_dateCheckIn, B_dateCheckOut, B_idTable, B_status) values (GETDATE(), GETDATE(), 6, 1);

select max(B_id) from dbo.Bill;

update dbo.Bill set B_status = 1 where B_id = 1;

alter table dbo.Bill add B_totalPrice float;

-- Bill_Info
insert into dbo.BillInfo(BI_idBill, BI_idBeverage, BI_count) values (4, 3, 2);
insert into dbo.BillInfo(BI_idBill, BI_idBeverage, BI_count) values (5, 1, 3);
insert into dbo.BillInfo(BI_idBill, BI_idBeverage, BI_count) values (6, 3, 2);

-- Beverage
insert into dbo.Beverage(BE_name, BE_idCategory, BE_price) values (N'Bạc xỉu', 1, 28000);
insert into dbo.Beverage(BE_name, BE_idCategory, BE_price) values (N'Trà sữa gạo', 2, 32000);
insert into dbo.Beverage(BE_name, BE_idCategory, BE_price) values (N'Trà lài hoa cúc', 3, 30000);
insert into dbo.Beverage(BE_name, BE_idCategory, BE_price) values (N'Bơ chuối hạt điều', 4, 25000);
insert into dbo.Beverage(BE_name, BE_idCategory, BE_price) values (N'Nước cam ép', 5, 15000);


-- Caterogy
insert into dbo.BeverageCategory(BC_name) values (N'Cà phê');
insert into dbo.BeverageCategory(BC_name) values (N'Trà sữa');
insert into dbo.BeverageCategory(BC_name) values (N'Trà');
insert into dbo.BeverageCategory(BC_name) values (N'Sinh tố');
insert into dbo.BeverageCategory(BC_name) values (N'Nước ép');

select * from TableBeverage;
select * from BeverageCategory;
select * from Beverage;
select * from Bill;
select * from BillInfo;
select * from TableBeverage;
select * from Account;

select BE_id as [ID], BE_idCategory as [ID danh mục], BE_name as [Tên đồ uống], BE_price as [Đơn giá] from Beverage;

delete BillInfo;
delete Bill;

select * from dbo.Bill where B_idTable = 5 and B_status = 0;
go


select be.BE_name, bi.BI_count, be.BE_price, be.BE_price*bi.BI_count as totalPrice from dbo.BillInfo as bi, dbo.Bill as b, dbo.Beverage as be
where bi.BI_idBill = b.B_id and bi.BI_idBeverage = be.BE_id and b.B_idTable = 5


-- procedure --
create proc USP_GetAccountByUserName
@userName nvarchar(100)
as
begin
	select * from dbo.Account where A_userName = @userName
end
go

exec dbo.USP_GetAccountByUserName @userName = N'admin'
go

create proc USP_Login
@userName nvarchar(100), @passWord nvarchar(100)
as
begin
	select * from dbo.Account where A_userName = @userName and A_passWord = @passWord
end
go

exec dbo.USP_Login @userName = N'admin', @passWord = N'1'
go

create proc USP_GetTableList
as select * from dbo.TableBeverage
go

exec dbo.USP_GetTableList

create proc USP_InsertBill
@idTable int
as
begin
	insert into dbo.Bill(B_dateCheckIn, B_dateCheckOut, B_idTable, B_status) values (GETDATE(), null, @idTable, 0);
end
go

alter proc USP_InsertBill
@idTable int
as
begin
	insert into dbo.Bill(B_dateCheckIn, B_dateCheckOut, B_idTable, B_status, B_discount) values (GETDATE(), null, @idTable, 0, 0);
end
go

create proc USP_InsertBillInfo
@idBill int, @idBeverage int, @count int
as
begin
	insert into dbo.BillInfo(BI_idBill, BI_idBeverage, BI_count) values (@idBill, @idBeverage, @count);
end
go

alter proc USP_InsertBillInfo
@idBill int, @idBeverage int, @count int
as
begin
	declare @isExitsBillInfo int
	declare @beverageCount int = 1

	select @isExitsBillInfo = BI_id, @beverageCount = bi.BI_count 
	from dbo.BillInfo as bi 
	where BI_idBill = @idBill and BI_idBeverage = @idBeverage;

	if(@isExitsBillInfo > 0)
	begin
		declare @newCount int = @beverageCount + @count
		if (@newCount > 0)
			update dbo.BillInfo set BI_count = @beverageCount + @count where BI_idBeverage = @idBeverage
		else
			delete dbo.BillInfo where BI_idBill = @idBill and BI_idBeverage = @idBeverage
	end
	else
	begin
		insert into dbo.BillInfo(BI_idBill, BI_idBeverage, BI_count) values (@idBill, @idBeverage, @count);
	end
end
go

-- Store Proceduce chuyển bàn --
create proc USP_SwitchTable
@idTable1 int, @idTable2 int
as
begin
	declare @idFirstBill int
	declare @idSecondBill int

	select @idSecondBill = B_id from dbo.Bill where B_idTable = @idTable2 and B_status = 0;
	select @idFirstBill = B_id from dbo.Bill where B_idTable = @idTable1 and B_status = 0;

	if (@idFirstBill = null)
	begin
		insert into dbo.Bill(B_dateCheckIn, B_dateCheckOut, B_idTable, B_status) values (getdate(), null, @idTable1, 0);

		select @idFirstBill = max(B_id) from dbo.Bill where B_idTable = @idTable1 and B_status = 0;

	end

	if (@idSecondBill = null)
	begin
		insert into dbo.Bill(B_dateCheckIn, B_dateCheckOut, B_idTable, B_status) values (getdate(), null, @idTable2, 0);

		select @idSecondBill = max(B_id) from dbo.Bill where B_idTable = @idTable2 and B_status = 0;

	end


	select BI_id into IDBillInfoTable from dbo.BillInfo where BI_idBill = @idSecondBill;
	update dbo.BillInfo set BI_idBill = @idSecondBill where BI_idBill = @idFirstBill;
	update dbo.BillInfo set BI_idBill = @idFirstBill where BI_id in (select * from IDBillInfoTable);

	drop table IDBillInfoTable;
end
go

alter proc USP_SwitchTable
@idTable1 int, @idTable2 int
as
begin
	declare @idFirstBill int
	declare @idSecondBill int

	declare @isFirstTableEmty int = 1
	declare @isSecondTableEmty int = 1

	select @idSecondBill = B_id from dbo.Bill where B_idTable = @idTable2 and B_status = 0;
	select @idFirstBill = B_id from dbo.Bill where B_idTable = @idTable1 and B_status = 0;

	if (@idFirstBill is null)
	begin
		insert into dbo.Bill(B_dateCheckIn, B_dateCheckOut, B_idTable, B_status) values (getdate(), null, @idTable1, 0);

		select @idFirstBill = max(B_id) from dbo.Bill where B_idTable = @idTable1 and B_status = 0;

	end

	select @isFirstTableEmty = count(*) from BillInfo where BI_idBill = @idFirstBill

	if (@idSecondBill is null)
	begin
		insert into dbo.Bill(B_dateCheckIn, B_dateCheckOut, B_idTable, B_status) values (getdate(), null, @idTable2, 0);

		select @idSecondBill = max(B_id) from dbo.Bill where B_idTable = @idTable2 and B_status = 0;

	end

	select @isSecondTableEmty = count(*) from BillInfo where BI_idBill = @idSecondBill

	select BI_id into IDBillInfoTable from dbo.BillInfo where BI_idBill = @idSecondBill;
	update dbo.BillInfo set BI_idBill = @idSecondBill where BI_idBill = @idFirstBill;
	update dbo.BillInfo set BI_idBill = @idFirstBill where BI_id in (select * from IDBillInfoTable);

	drop table IDBillInfoTable;

	if (@isFirstTableEmty = 0)
		update dbo.TableBeverage set T_status = N'Trống' where T_id = @idTable2

	if (@isSecondTableEmty = 0)
		update dbo.TableBeverage set T_status = N'Trống' where T_id = @idTable1
end
go

exec dbo.USP_SwitchTable @idTable1 = 6, @idTable2 = 5;

alter proc USP_GetListBillByDate
@checkIn date, @checkOut date
as
begin
	select t.T_name as [Tên bàn], b.B_totalPrice as [Tổng tiền], B_dateCheckIn as [Ngày vào], B_dateCheckOut as [Ngày ra], B_discount as [Khuyến mãi] 
	from dbo.Bill as b, dbo.TableBeverage as t, dbo.BillInfo as bi, dbo.Beverage as be
	where B_dateCheckIn >= @checkIn and B_dateCheckOut <= @checkOut and b.B_status = 1
	and t.T_id = b.B_idTable and bi.BI_idBill = b.B_id and bi.BI_idBeverage = be.BE_id;
end
go

create proc USP_UpdateAccount
@userName nvarchar(100), @displayName nvarchar(100), @password nvarchar(100), @newPassword nvarchar(100)
as
begin
	declare @isRightPass int = 0;

	select @isRightPass = count(*) from dbo.Account where A_userName = @userName and A_passWord = @password

	if(@isRightPass = 1)
	begin
		if(@newPassword = null or @newPassword = '')
		begin
			update dbo.Account set A_displayName = @displayName where A_userName = @userName
		end
		else
			update dbo.Account set A_displayName = @displayName, A_passWord = @newPassword where A_userName = @userName
	end
end
go

-- trigger

alter trigger UTG_UpdateBillInfo
on dbo.BillInfo for insert, update
as
begin
	declare @idBill int

	select @idBill = BI_idBill from inserted

	declare @idTable int

	select @idTable = B_idTable from dbo.Bill where B_id = @idBill and B_status = 0

	declare @count int
	select @count = count(*) from dbo.BillInfo where BI_idBill = @idBill

	if (@count > 0)
		update dbo.TableBeverage set T_status = N'Có người' where T_id = @idTable
	else 
		update dbo.TableBeverage set T_status = N'Trống' where T_id = @idTable
end
go

create trigger UTG_UpdateBill
on dbo.Bill for update
as
begin
	declare @idBill int
	select @idBill = B_id from inserted

	declare @idTable int
	select @idTable = B_idTable from dbo.Bill where B_id = @idBill

	declare @count int = 0
	select @count = count(*) from dbo.Bill where B_idTable = @idTable and B_status = 0

	if(@count = 0)
		update dbo.TableBeverage set T_status = N'Trống'
end
go

alter trigger UTG_UpdateBill
on dbo.Bill for update
as
begin
	declare @idBill int
	select @idBill = B_id from inserted

	declare @idTable int
	select @idTable = B_idTable from dbo.Bill where B_id = @idBill

	declare @count int = 0
	select @count = count(*) from dbo.Bill where B_idTable = @idTable and B_status = 0

	if(@count = 0)
		update dbo.TableBeverage set T_status = N'Trống' where T_id = @idTable
end
go

GO
CREATE TRIGGER UTG_DeleteBillInfo
ON dbo.BillInfo FOR DELETE
AS
BEGIN
	DECLARE @idBillInfo INT
	DECLARE @idBill INT
	SELECT @idBillInfo = BI_id , @idBill = deleted.BI_idBill From deleted

	DECLARE @idTable INT
	Select @idTable = B_idTable FROM dbo.Bill WHERE B_id = @idBill

	DECLARE @count INT = 0

	SELECT @count = COUNT(*) FROM dbo.BillInfo as bi, dbo.Bill as b WHERE b.B_id = bi.BI_idBill AND b.B_id = @idBill AND B.B_status = 0

	IF(@count = 0)
		UPDATE dbo.TableBeverage SET T_status = N'Trống' WHERE T_id = @idTable
END
GO

CREATE PROC USP_GetListBillByDateAndPage
@checkIn date, @checkOut date, @page int
as
begin
	DECLARE @pageRows INT = 10
    DECLARE @selectRows INT =@pageRows
    DECLARE @exceptRows INT = (@page - 1) * @pageRows

	;WITH BillShow AS (
	select b.B_id, t.T_name as [Tên bàn], b.B_totalPrice as [Tổng tiền], B_dateCheckIn as [Ngày vào], B_dateCheckOut as [Ngày ra], B_discount as [Khuyến mãi] 
	from dbo.Bill as b, dbo.TableBeverage as t
	where B_dateCheckIn >= @checkIn and B_dateCheckOut <= @checkOut and b.B_status = 1
	and t.T_id = b.B_idTable)

	SELECT TOP (@selectRows) * FROM BillShow where B_id NOT IN (SELECT TOP (@exceptRows) B_id FROM BillShow)
end
go
select * from bill

exec dbo.USP_GetListBillByDateAndPage @checkIn = '2025-03-26', 
@checkOut = '2025-03-26',
@page = 3

exec dbo.USP_GetListBillByDate @checkIn = '2025-03-25', 
@checkOut = '2025-03-25'

CREATE proc USP_GetNumBillListByDate
@checkIn date, @checkOut date
as
begin
	select COUNT(*)
	from dbo.Bill as b, dbo.TableBeverage as t
	where B_dateCheckIn >= @checkIn and B_dateCheckOut <= @checkOut and b.B_status = 1
	and t.T_id = b.B_idTable
end
go

