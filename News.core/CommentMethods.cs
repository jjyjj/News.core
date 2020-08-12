using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News.core
{
    public class CommentMethods
    {
        public string CreateCode(int codeLength)
        {
            //这里本人没做是否允许重复控制，读者需要可做相应控制
            string so = "1,2,3,4,5,6,7,8,9,0,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            string[] strArr = so.Split(',');
            string code = "";
            Random rand = new Random();
            for (int i = 0; i < codeLength; i++)
            {
                code += strArr[rand.Next(0, strArr.Length)];
            }
            return code;
        }

    }
}
