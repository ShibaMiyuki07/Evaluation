create sequence idadmin;
create sequence idbien;
create sequence idtypebien;
create sequence idclient;
create sequence idlocation;


create table admin(idadmin char(10) primary key default concat('AD00',nextval('idadmin')),login char(50),mdp char(20));

create table client(idclient char(10) primary key default concat('C00',nextval('idclient')),numeroclient char(11),emailclient char(255));

create table typebien(idtypebien char(10) primary key default concat('T00',nextval('idtypebien')),type char(100),commission decimal(3,1) default '0.0');

create table biens(idbien char(10) primary key default concat('B00',nextval('idbien')),nombien char(255),description text,region char(200),loyer decimal(10,2),photos text,idproprietaire char(10),idtypebien char(10),foreign key(idproprietaire) references client(idclient),foreign key(idtypebien) references typebien(idtypebien));

create table location(idlocation char(10) primary key default concat('L00',nextval('idlocation')),idclient char(10),duree int,datedebut date,idbien char(10),foreign key(idclient) references client(idclient),foreign key(idbien) references biens(idbien));


insert into admin(login,mdp) values('test','test');
insert into client(numeroclient) values('0347001943');
insert into client(emailclient) values('test@test.mg');

delete from client;
delete from typebien;
delete from biens;
delete from location;