using System;
using System.Collections.Generic;
using System.Linq;

namespace Administrator.Department.Helpers
{
    public class DepartmentHelper
    {
        #region Hàm xử lý phân cấp đơn vị
        public IEnumerable<Administrator.Department.Models.Administrator_Department> SetLevelParentChildren(IEnumerable<Administrator.Department.Models.Administrator_Department> list, int parentId)
        {
            string name = "|";
            List<int> array = new List<int>();
            List<int> clone = new List<int>();
            clone.Add(parentId);
            do
            {
                var dr1 = list.Where(item => clone.Contains(item.ParentId));
                if (dr1.Count() > 0)
                {
                    name += "----";
                    foreach (var dataRow in dr1)
                    {
                        dataRow.DepartmentName = name + dataRow.DepartmentName;
                        array.Add(dataRow.DepartmentId);
                    }
                }
                clone = new List<int>();
                clone = array;
                array = new List<int>();
            }
            while (clone.Count > 0);
            return SortParentChildren(list);
        }

        public IEnumerable<Administrator.Department.Models.Administrator_Department> SortParentChildren(IEnumerable<Administrator.Department.Models.Administrator_Department> list)
        {
            List<Administrator.Department.Models.Administrator_Department> pc = new List<Administrator.Department.Models.Administrator_Department>();
            var stack = new Stack<Administrator.Department.Models.Administrator_Department>();
            var menuModel = list.Where(item => item.ParentId == 0).OrderByDescending(item => item.DepartmentIndex);
            foreach (var itemInList in menuModel)
            {
                stack.Push(itemInList);
            }
            while (stack.Any())
            {
                var curentModel = stack.Pop();
                var curentMenuModel = list.Where(item => item.ParentId == curentModel.DepartmentId).OrderByDescending(item => item.DepartmentIndex);
                foreach (var itemInList in curentMenuModel)
                {
                    stack.Push(itemInList);
                }
                pc.Add(curentModel);
            }
            return pc;
        }
        #endregion

        public List<Administrator.Department.Models.Administrator_Department> GetAllChild(List<Administrator.Department.Models.Administrator_Department> listDepartment, int departmentId)
        {
            List<Administrator.Department.Models.Administrator_Department> result = listDepartment.Where(item => item.ParentId == departmentId).ToList();
            List<Administrator.Department.Models.Administrator_Department> childs = new List<Models.Administrator_Department>();
            childs.AddRange(result);
            
            if (result.Count > 0)
                foreach (var item in result)
                {
                    if (GetAllChild(listDepartment, item.DepartmentId).Count > 0)
                        childs.AddRange(GetAllChild(listDepartment, item.DepartmentId).ToList());
                }
            return childs;
        }
    }
}