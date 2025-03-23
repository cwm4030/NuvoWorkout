drop table if exists nw_config cascade;
create table nw_config(
    id bigint not null primary key generated always as identity,
    name varchar(128) null,
    description varchar(256) null,
    version varchar(128) null
);

drop table if exists nw_user cascade;
create table nw_user(
    id bigint not null primary key generated always as identity,
    inactive boolean null,
    date_created timestamp with time zone null,
    date_updated timestamp with time zone null,
    username varchar(128) not null unique,
    password_hash varchar(512) not null,
    first_name varchar(128) null,
    middle_name varchar(128) null,
    last_name varchar(128) null,
    sex varchar(32) null,
    age int null,
    weight_metric varchar(32) null,
    weight decimal null,
    body_fat_percentage decimal null
);

drop table if exists nw_user_program cascade;
create table nw_user_program(
    id bigint not null primary key generated always as identity,
    inactive boolean null,
    name varchar(128) null,
    description varchar(256) null,
    start_date timestamp with time zone null,
    end_date timestamp with time zone null,
    nw_user_id bigint null references nw_user(id)
);

drop table if exists nw_user_session cascade;
create table nw_user_session(
    id bigint not null primary key generated always as identity,
    inactive boolean null,
    name varchar(128) null,
    description varchar(256) null,
    session_number int null,
    nw_user_program_id bigint null references nw_user_program(id),
    unique(nw_user_program_id, week_number, session_number)
);

drop table if exists nw_user_workout cascade;
create table nw_user_workout(
    id bigint not null primary key generated always as identity,
    inactive boolean null,
    name varchar(128) null,
    description varchar(256) null,
    workout_number int null,
    nw_user_session_id bigint null references nw_user_session(id),
    unique(nw_user_session_id, workout_number)
);

drop table if exists nw_user_set cascade;
create table nw_user_set(
    id bigint not null primary key generated always as identity,
    inactive boolean null,
    name varchar(128) null,
    description varchar(256) null,
    set_number int null,
    nw_user_workout_id bigint null references nw_user_workout(id)
);