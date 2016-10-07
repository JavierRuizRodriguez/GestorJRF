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
                    ISqlMapper mapa = Mapper.Instance();
                    return mapa;
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
