using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FuncPractice
{
    class Program
    {
        static void Main(string[] args)
        {
            Data<Fake> repo = new Data<Fake>();

            repo.Add(new Fake() { Name = "aaa", Children = 0 });
            repo.Add(new Fake() { Name = "bbb", Children = 8 });
            repo.Add(new Fake() { Name = "ccc", Children = 6 });

            var result = repo.GetSingle(k => k.Name.Equals("ccc") || k.Children > 0);

            result.ForEach((item) => Console.WriteLine(item.Name));

        }
    }

    public class Fake
    {
        public string Name { get; set; }
        public int Children { get; set; }
    }

    public class Data<T> where T : Fake
    {
        public List<T> Warehouse = new List<T>();

        public void Add(T item)
        {
            Warehouse.Add(item);
        }

        public List<T> GetSingle(Expression<Func<T, bool>> predicate)
        {
            // `return` could be ommitted if only one statement exists in the BODY of ANONYMOUS method.
            // Func<Expression<Func<T, bool>>, List<T>> invokeFunction = (key) => Warehouse.AsQueryable().Where(key).ToList();

            // Func => Multiple 
            Func<Expression<Func<T, bool>>, List<T>> invokeFunction = key => 
                {
                    // throw new ArgumentOutOfRangeException("doeoe");

                    Console.WriteLine("statement is executed at " + DateTime.Now.ToString());
                    return Warehouse.AsQueryable().Where(key).ToList();             
                }; 

            return TryGetFunction(invokeFunction, predicate);
        }

        
        private List<T> TryGetFunction(Func<Expression<Func<T, bool>>, List<T>> invokeFunction, Expression<Func<T, bool>> predicate)
        {
            List<T> result = null;
            
            try
            {
                result = invokeFunction.Invoke(predicate);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }

    }
}
