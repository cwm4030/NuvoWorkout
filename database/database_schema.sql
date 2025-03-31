drop table if exists nw_config cascade;
create table nw_config(
    id bigint not null primary key generated always as identity,
    inactive boolean null,
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
    birth_date timestamp with time zone null,
    sex varchar(32) null,
    weight_metric varchar(32) null,
    weight decimal null,
    body_fat_percentage decimal null
);

drop table if exists workout_definition cascade;
create table workout_definition(
    id bigint not null primary key generated always as identity,
    inactive boolean null,
    name varchar(128) null,
    description varchar(256) null,
    category varchar(256) null,
    url varchar(256) null,
    nw_user_id bigint null references nw_user(id)
);

drop table if exists workout_routine cascade;
create table workout_routine(
    id bigint not null primary key generated always as identity,
    inactive boolean null,
    date_created timestamp with time zone null,
    date_updated timestamp with time zone null,
    name varchar(128) null,
    description varchar(256) null,
    number_of_weeks int null,
    current_week int null,
    current_session int null,
    nw_user_id bigint null references nw_user(id)
);

drop table if exists workout_session cascade;
create table workout_session(
    id bigint not null primary key generated always as identity,
    inactive boolean null,
    name varchar(128) null,
    description varchar(256) null,
    week_number int null,
    session_number int null,
    workout_routine_id bigint null references workout_routine(id),
    unique(workout_routine_id, week_number, session_number)
);

drop table if exists workout cascade;
create table workout(
    id bigint not null primary key generated always as identity,
    inactive boolean null,
    sets_and_reps varchar(512) null,
    workout_definition_id bigint null references workout_definition(id),
    workout_session_id bigint null references workout_session(id)
);