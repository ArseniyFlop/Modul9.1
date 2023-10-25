using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StudentManager
{
    class Student
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public double Mark { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var students = new List<Student>();

            while (true)
            {
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1. Добавить студента");
                Console.WriteLine("2. Удалить студента");
                Console.WriteLine("3. Редактировать студента");
                Console.WriteLine("4. Поиск студента");
                Console.WriteLine("5. Сортировка студентов");
                Console.WriteLine("6. Сохранить в файл");
                Console.WriteLine("7. Загрузить из файла");
                Console.WriteLine("8. Выход");

                switch (Console.ReadLine())
                {
                    case "1":
                        AddStudent(students);
                        Console.Clear();
                        break;
                    case "2":
                        DeleteStudent(students);
                        Console.Clear();
                        break;
                    case "3":
                        EditStudent(students);
                        Console.Clear();
                        break;
                    case "4":
                        SearchStudent(students);
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "5":
                        SortStudents(students);
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    case "6":
                        SaveToFile(students);
                        Console.Clear();
                        break;
                    case "7":
                        students = LoadFromFile();
                        Console.Clear();
                        break;
                    case "8":
                        return;
                    default:
                        Console.WriteLine("Неправильный ввод. Попробуйте еще раз.");
                        break;
                }
            }
        }

        private static void AddStudent(List<Student> students)
        {
            Console.WriteLine("Введите ФИО студента:");
            var name = Console.ReadLine();

            Console.WriteLine("Введите номер группы студента:");
            var group = Console.ReadLine();

            double mark;
            while (true)
            {
                Console.WriteLine("Введите средний балл студента:");
                if (double.TryParse(Console.ReadLine(), out mark) && mark >= 1.0 && mark <= 10.0)
                {
                    break;
                }
                Console.WriteLine("Неправильный ввод среднего балла. Введите число от 1.0 до 10.0.");
            }

            students.Add(new Student { Name = name, Group = group, Mark = mark });
            Console.WriteLine("Студент добавлен.");
        }

        private static void DeleteStudent(List<Student> students)
        {
            Console.WriteLine("Введите номер группы студента, которого хотите удалить:");
            var group = Console.ReadLine();

            var student = students.FirstOrDefault(s => s.Group == group);
            if (student != null)
            {
                students.Remove(student);
                Console.WriteLine("Студент удален.");
            }
            else
            {
                Console.WriteLine("Студента с такой группой не найдено.");
            }
        }

        private static void EditStudent(List<Student> students)
        {
            Console.WriteLine("Введите номер группы студента, которого хотите изменить:");
            var group = Console.ReadLine();

            var studentsWithGroup = students.Where(s => s.Group == group).ToList();

            if (studentsWithGroup.Count > 0)
            {
                Console.WriteLine("Выберите студента для редактирования (введите индекс):");
                for (int i = 0; i < studentsWithGroup.Count; i++)
                {
                    Console.WriteLine($"{i}. {studentsWithGroup[i].Name}, группа: {studentsWithGroup[i].Group}, средний балл: {studentsWithGroup[i].Mark}");
                }

                if (int.TryParse(Console.ReadLine(), out int selectedIndex) && selectedIndex >= 0 && selectedIndex < studentsWithGroup.Count)
                {
                    var student = studentsWithGroup[selectedIndex];
                    Console.WriteLine("Введите новое ФИО студента:");
                    student.Name = Console.ReadLine();

                    Console.WriteLine("Введите новый номер группы студента:");
                    student.Group = Console.ReadLine();

                    double mark;
                    while (true)
                    {
                        Console.WriteLine("Введите новый средний балл студента:");
                        if (double.TryParse(Console.ReadLine(), out mark) && mark >= 1.0 && mark <= 10.0)
                        {
                            student.Mark = mark;
                            break;
                        }
                        Console.WriteLine("Неправильный ввод среднего балла. Введите число от 1.0 до 10.0.");
                    }

                    Console.WriteLine("Студент изменен.");
                }
                else
                {
                    Console.WriteLine("Неправильный индекс студента.");
                }
            }
            else
            {
                Console.WriteLine("Студентов с такой группой не найдено.");
            }
        }

        private static void SearchStudent(List<Student> students)
        {
            Console.WriteLine("Введите критерий поиска (ФИО, группа, средний балл):");
            var searchTerm = Console.ReadLine();

            var foundStudents = students.Where(s => s.Name.Contains(searchTerm) || s.Group.Contains(searchTerm) || s.Mark.ToString().Contains(searchTerm)).ToList();

            if (foundStudents.Any())
            {
                Console.WriteLine("Результаты поиска:");
                foreach (var student in foundStudents)
                {
                    Console.WriteLine($"{student.Name}, группа: {student.Group}, средний балл: {student.Mark}");
                }
            }
            else
            {
                Console.WriteLine("Студентов с такими критериями не найдено.");
            }
        }

        private static void SortStudents(List<Student> students)
        {
            Console.WriteLine("Выберите параметр для сортировки (ФИО, группа, средний балл):");
            var sortBy = Console.ReadLine();

            switch (sortBy)
            {
                case "ФИО":
                    students = students.OrderBy(s => s.Name).ToList();
                    break;
                case "группа":
                    students = students.OrderBy(s => s.Group).ToList();
                    break;
                case "средний балл":
                    students = students.OrderBy(s => s.Mark).ToList();
                    break;
                default:
                    Console.WriteLine("Неправильный ввод. Попробуйте еще раз.");
                    return;
            }

            Console.WriteLine("Студенты отсортированы.");
            foreach (var student in students)
            {
                Console.WriteLine($"{student.Name}, группа: {student.Group}, средний балл: {student.Mark}");
            }
        }

        private static void SaveToFile(List<Student> students)
        {
            using (var file = new StreamWriter("students.txt"))
            {
                foreach (var student in students)
                {
                    file.WriteLine($"{student.Name};{student.Group};{student.Mark}");
                }
            }
        }

        private static List<Student> LoadFromFile()
        {
            var students = new List<Student>();

            if (File.Exists("students.txt"))
            {
                using (var file = new StreamReader("students.txt"))
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        var parts = line.Split(';');
                        if (parts.Length == 3)
                        {
                            students.Add(new Student { Name = parts[0], Group = parts[1], Mark = Convert.ToDouble(parts[2]) });
                        }
                    }
                }
            }

            return students;
        }
    }
}
