using IBatisNet.DataMapper;
using System;

namespace GestorJRF.MyBatis.NET
{
    class InstanciaPostgreSQL
    {
        public static ISqlMapper CogerInstaciaPostgreSQL
        {
            get
            {
                try
                {
                    return Mapper.Instance();
                }
                catch (Exception ex)
                {
                    ex.ToString();
                    throw ex;
                }
            }
        }
    }
}
