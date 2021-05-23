using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace KPI.DB.Database.Migrations.Migrations
{
    [Migration(202105101734)]
    public class NewScheme : Migration
    {
        private const string TypeEnum = "person_enum_type";

        public override void Down()
        {
            throw new NotImplementedException();
        }

        private readonly HashSet<string> personTypes = new HashSet<string>
        {
            "Student",
            "Teacher"
        };

        public override void Up()
        {
            CreateEnum(this, TypeEnum, personTypes);

            Execute.Sql(@"
                    CREATE TABLE public.VersionInfo (
                        Version int8 NOT NULL,
                        AppliedOn timestamp NULL,
                        Description varchar(1024) NULL
                    );

                    CREATE UNIQUE INDEX UC_Version ON public.VersionInfo USING btree(Version);

                    CREATE TABLE public.groups (
                        name text NOT NULL,
                        CONSTRAINT PK_groups PRIMARY KEY(name)
                    );

                    CREATE TABLE public.home_assignments(
                        deadline timestamp NOT NULL,
                        points int4 NOT NULL,
                        id serial NOT NULL,
                        CONSTRAINT PK_home_assignments PRIMARY KEY (id)
                    );

                    CREATE TABLE public.lessons(
                        time timestamp NOT NULL,
                        id serial NOT NULL,
                        theme text NOT NULL,
                        CONSTRAINT PK_lessons PRIMARY KEY (id)
                    );

                    CREATE TABLE public.persons(
                        name text NOT NULL,
                        email text NOT NULL,
                        id serial NOT NULL,
                        type person_enum_type NOT NULL,
                        CONSTRAINT PK_persons PRIMARY KEY (id)
                    );

                    CREATE TABLE public.subjects(
                        name text NOT NULL,
                        CONSTRAINT PK_subjects PRIMARY KEY (name)
                    );

                    CREATE TABLE public.person_assignments(
                    person_id serial NOT NULL,
                    home_assignment int4 NOT NULL,
                        CONSTRAINT FK_person_assignments_home_assignment_home_assignments_id FOREIGN KEY (home_assignment) REFERENCES home_assignments(id),
                        CONSTRAINT FK_person_assignments_person_id_persons_id FOREIGN KEY (person_id) REFERENCES persons(id)
                    );

                    CREATE TABLE public.person_lesson(
                    person_id serial NOT NULL,
                    lesson_id serial NOT NULL,
                        CONSTRAINT FK_person_lesson_lesson_id_lessons_id FOREIGN KEY (lesson_id) REFERENCES lessons(id),
                        CONSTRAINT FK_person_lesson_person_id_persons_id FOREIGN KEY (person_id) REFERENCES persons(id)
                    );

                    CREATE TABLE public.persons_groups(
                    person_id serial NOT NULL,
                    group_name text NOT NULL,
                    CONSTRAINT FK_persons_groups_group_name_groups_name FOREIGN KEY (group_name) REFERENCES groups(name),
                        CONSTRAINT FK_persons_groups_person_id_persons_id FOREIGN KEY (person_id) REFERENCES persons(id)
                    );
");

        }
        public static void CreateEnum(Migration migration, string enumName, IEnumerable<string> values)
        {
            string valuesList = string.Join(", ", values.Select(s => $"'{s}'"));
            migration.Execute.Sql($"CREATE TYPE {enumName} AS ENUM ({valuesList})");
        }
    }
}
