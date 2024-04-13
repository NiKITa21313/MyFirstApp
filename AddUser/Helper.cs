using AddUser.Model;
using System.Data.Entity;

namespace AddUser
{
    /// <summary>
    /// Класс, предназначенный для работы с БД
    /// </summary>
    public class Helper
    {
        /// <summary>
        /// переменная контекста БД
        /// </summary>
        private static telekommunikationEntities s_telekommunikationEntities;
        /// <summary>
        /// Метод для полчения контекста БД
        /// </summary>
        /// <returns>Вернет подключение к БД</returns>
        public static telekommunikationEntities GetContext()
        {
            if (s_telekommunikationEntities == null)
            {
                s_telekommunikationEntities = new telekommunikationEntities();
            }
            return s_telekommunikationEntities;
        }
        public void CreateSotrudnik(Sotrudniki sotrudnik)
        {
            s_telekommunikationEntities.Sotrudniki.Add(sotrudnik);
            s_telekommunikationEntities.SaveChanges();
        }
        public void CreateUsers(User user) 
        {
            s_telekommunikationEntities.User.Add(user);
            s_telekommunikationEntities.SaveChanges();
        }
        public void UpdateUsers(Sotrudniki users)
        {
            s_telekommunikationEntities.Entry(users).State = EntityState.Modified;
            s_telekommunikationEntities.SaveChanges();
        }
        public void RemoveUsers(int idUsers)
        {
            var users = s_telekommunikationEntities.Sotrudniki.Find(idUsers);
            s_telekommunikationEntities.Sotrudniki.Remove(users);
            s_telekommunikationEntities.SaveChanges();
        }
    }
}
