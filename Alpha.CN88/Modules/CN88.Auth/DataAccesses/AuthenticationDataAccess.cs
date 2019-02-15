namespace CN88.Auth.DataAccesses
{
    using System;
    using System.Collections;
    using System.Data;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public interface AuthenticationDataAccess
    {
        bool IsShowBetChoiceLimit(ICriteria criteria);

        DataTable ExecuteDataTable(ICriteria criteria);

        Hashtable ExecuteNonQuery(ICriteria criteria);
    }

    public class AuthenticationDataAccessImpl : AuthenticationDataAccess
    {
        public bool IsShowBetChoiceLimit(ICriteria criteria)
        {
            using (IObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader.Read())
                    {
                        return Convert.ToBoolean(reader[0]);
                    }
                }
            }

            return false;
        }

        public DataTable ExecuteDataTable(ICriteria criteria)
        {
            using (IObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                var dataSet =  db.ExecuteDataSet(criteria.GetSpParams());

                return dataSet.Tables[0];
            }
        }

        public Hashtable ExecuteNonQuery(ICriteria criteria)
        {
            using (IObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());

                return db.GetOuputParams();
            }
        }
    }
}
