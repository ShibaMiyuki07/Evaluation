create sequence idadmin;
create sequence idbien;
create sequence idtypebien;
create sequence idclient;
create sequence idlocation;
create sequence idpaye;
create sequence idlocationparmois;

alter sequence idadmin minvalue 0 restart with 1 ;
alter sequence idbien minvalue 0 restart with 1;
alter sequence idtypebien minvalue 0 restart with 1;
alter sequence idclient minvalue 0 restart with 1;
alter sequence idlocation minvalue 0 restart with 1;
alter sequence idlocationparmois minvalue 0 restart with 1;



delete from client;
delete from typebien;
delete from biens;
delete from location;
delete from paye;
delete from locationparmois;


create table admin(idadmin char(10) primary key default concat('AD00',nextval('idadmin')),login char(50),mdp char(20));

create table client(idclient char(10) primary key default concat('C00',nextval('idclient')),numeroclient char(11),emailclient char(255));

create table typebien(idtypebien char(10) primary key default concat('T00',nextval('idtypebien')),type char(100),commission decimal(4,2) default '0.0');

create table biens(idbien char(10) primary key default concat('B00',nextval('idbien')),nombien char(255),description text,region char(200),loyer decimal(10,2),photos text,idproprietaire char(10),idtypebien char(10),foreign key(idproprietaire) references client(idclient),foreign key(idtypebien) references typebien(idtypebien));

create table location(idlocation char(10) primary key default concat('L00',nextval('idlocation')),idclient char(10),duree int,datedebut date,idbien char(10),foreign key(idclient) references client(idclient),foreign key(idbien) references biens(idbien));

create table paye(idpaye char(10) primary key default concat('P00',nextval('idpaye')),idlocation char(10),moispaye int,anneepaye int, foreign key(idlocation) references location(idlocation));

create table locationparmois(idlocationparmois char(10) primary key default concat('I00',nextval('idlocationparmois')),mois int,annee int,montant decimal(10,2),idlocation char(10),commission decimal(5,2),foreign key(idlocation) references location(idlocation));

insert into admin(login,mdp) values('test','test');
insert into client(numeroclient) values('0347001943');
insert into client(emailclient) values('test@test.mg');
insert into client(emailclient) values('test2@test.mg');
insert into client(numeroclient) values('0384124196');

insert into typebien(type,commission) values('maison','10'),('appartement','12'),('villa','15'),('immeuble','20');

insert into biens(nombien,description,region,loyer,photos,idproprietaire,idtypebien) values('test1','test1','region1','140000','','C001','T001');

insert into biens(nombien,description,region,loyer,photos,idproprietaire,idtypebien) values('test2','test2','region2','1200000','','C004','T004');


insert into location(idclient,duree,datedebut,idbien) values('C002',3,'2024-01-01','B001');

insert into location(idclient,duree,datedebut,idbien) values('C002',4,'2024-06-01','B001');

insert into location(idclient,duree,datedebut,idbien) values('C003',6,'2024-02-01','B002');

insert into paye(idlocation,moispaye,anneepaye) values('L001','01','2024');






delete from client;
delete from typebien;
delete from biens;
delete from location;