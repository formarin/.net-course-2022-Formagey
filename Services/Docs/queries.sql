create extension if not exists "uuid-ossp";

--а) наполнить базу тестовыми данными пользуясь оператором Insert;

insert into client
values
       (uuid_generate_v4(), 'firstName01', 'lastName01', '2000-01-01', 77700001, 77700001, 12),
       (uuid_generate_v4(), 'firstName02', 'lastName02', '2000-02-02', 77700002, 77700002, 14),
       (uuid_generate_v4(), 'firstName03', 'lastName03', '2000-03-03', 77700003, 77700003, 14);

insert into employee
values
       (uuid_generate_v4(), 'firstName04', 'lastName04', '2000-04-04', 77700004, 77700004, 12, null, 123),
       (uuid_generate_v4(), 'firstName05', 'lastName05', '2000-05-05', 77700005, 77700005, 14, null, 456),
       (uuid_generate_v4(), 'firstName06', 'lastName06', '2000-06-06', 77700006, 77700006, 14, null, 789);

insert into account
values
       (uuid_generate_v4(), 'USD', 720, 'dc073924-7980-427c-b4fa-13bb1b41618e'),
       (uuid_generate_v4(), 'EUR', 92, 'dc073924-7980-427c-b4fa-13bb1b41618e'),
       (uuid_generate_v4(), 'USD', 102, 'bc3bdade-561e-4cb2-ad72-50e01cfd18cb'),
       (uuid_generate_v4(), 'EUR', 710, 'bc3bdade-561e-4cb2-ad72-50e01cfd18cb'),
       (uuid_generate_v4(), 'USD', 30, '2de6a30b-f10b-4ae6-8271-c32e98de606c'),
       (uuid_generate_v4(), 'EUR', 1189, '2de6a30b-f10b-4ae6-8271-c32e98de606c');


--б) провести выборки клиентов, у которых сумма на счету ниже
--определенного значения, отсортированных в порядке возрастания суммы;

select * from account;

select c.first_name, c.last_name, a.amount, a.currency_name
from account a
join client c on c.id = a.client_id
where a.amount<400
order by a.amount;

--в) провести поиск клиента с минимальной суммой на счете;

select c.first_name, c.last_name, a.amount, a.currency_name
from account a
join client c on c.id = a.client_id
where a.amount=(select min(a.amount) from account a);

--г) провести подсчет суммы денег у всех клиентов банка;

select sum(a.amount), a.currency_name
from account a
group by a.currency_name;

--д) с помощью оператора Join, получить выборку банковских счетов и
--их владельцев;

select c.first_name, c.last_name, a.amount, a.currency_name
from account a
join client c on c.id = a.client_id;

--е) вывести клиентов от самых старших к самым младшим;

select date_of_birth, *
from client
order by client.date_of_birth;

--ж) подсчитать количество человек, у которых одинаковый возраст;
--з) сгруппировать клиентов банка по возрасту;

select count(c), date_part('year', age(c.date_of_birth))
from client c
group by date_part('year', age(c.date_of_birth));

--и) вывести только N человек из таблицы.

select *
from client
limit 2;