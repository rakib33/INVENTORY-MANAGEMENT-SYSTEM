using BookStore.Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Repository
{
    public class ImageRepository :IImage
    {
        public UserImage SetImage(HttpPostedFileBase file)
        {

            UserImage SetUserImage = new UserImage();

          
            if (file != null)
            {
               SetUserImage.ImageStream = new byte[file.ContentLength];
               file.InputStream.Read(SetUserImage.ImageStream, 0, file.ContentLength);

            
               return SetUserImage;                 

            }
            return null;
           

        }

    }
}