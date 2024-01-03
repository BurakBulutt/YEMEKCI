create table Customer (
ID int not null primary key identity(1,1),
name nvarchar(30) not null,
surname nvarchar(15) not null,
birthdate date not null,
email nvarchar(100) not null,
password nvarchar(50) not null,
rolName nvarchar(3) not null
)

create table Address (
ID int not null primary key identity(1,1),
city nvarchar(30) not null,
district nvarchar(30) not null,
postalCode int not null,
address1 nvarchar(max) not null,
customerID int not null,

constraint fk_customerID_inCustomers foreign key (customerID) references Customer(ID)
)

create table Restaurant(
ID int not null primary key identity(1,1),
restaurant_address_id int not null,
name nvarchar(150) not null,

constraint fk_restaurant_addressID foreign key (restaurant_address_id) references Restaurant_Address(ID)
)

create table Restaurant_Address (
ID int not null primary key identity(1,1),
city nvarchar(30) not null,
district nvarchar(30) not null,
postalCode int not null,
address nvarchar(max) not null,
)


create table Dish(
ID int not null primary key identity(1,1),
restaurantID int not null,
categoryID int not null,
name nvarchar(50) not null,
unit_price money not null,

constraint fk_restaurantID_inDishs foreign key (restaurantID) references Restaurant(ID),
constraint fk_categoryID_inDishs foreign key (categoryID) references Category(ID)
)

create table Category(
ID int not null primary key identity(1,1),
name nvarchar(50) not null,
)

create table Payment(
ID int not null primary key identity(1,1),
name nvarchar(30) not null,
)

create table Discount(
ID int not null primary key identity(1,1),
name nvarchar(30) not null,
discount_rate money not null,
)

create table CustomersDiscounts(
customerID int not null,
discountID int not null,

constraint pk_CustomersDiscounts primary key (customerID, discountID),
constraint fk_customerID foreign key (customerID) references Customer(ID),
constraint fk_discountID foreign key (discountID) references Discount(ID)
)

create table Cart(
ID int not null primary key identity(1,1),
customerID int not null,

constraint fk_customerID_inCarts foreign key (customerID) references Customer(ID)
)

create table CartItem(
ID int not null primary key identity(1,1),
cartID int not null,
dishID int not null,
quantity int not null,

constraint fk_cartID_inCartItem foreign key (cartID) references Cart(ID),
constraint fk_dishID_inCartItem foreign key (dishID) references Dish(ID)
)

create table Orderr(
ID int not null primary key identity(1,1),
customerID int not null,
paymentID int not null,
order_date date not null,
total_Amaount money not null,
order_status nvarchar(30) not null,
addressID int not null,

constraint fk_customerID_inOrderr foreign key (customerID) references Customer(ID),
constraint fk_paymentID_inOrderr foreign key (paymentID) references Payment(ID),
constraint fk_addressID_inOrderr foreign key (addressID) references Address(ID)
)


create table OrderItem(
ID int not null primary key identity(1,1),
orderID int not null,
dishID int not null,
quantity int not null,

constraint fk_orderID_inOrderItemm foreign key (orderID) references Orderr(ID),
constraint fk_dishID_inOrderItemm foreign key (dishID) references Dish(ID)
)
