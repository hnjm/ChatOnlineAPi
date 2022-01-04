﻿using AutoMapper;
using ChatOnline.Application.Common.Mappings;
using ChatOnline.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatOnline.Application.Users.GetUserDetail.Queries
{
    public class UserDetailViewModel : IMapFrom<User>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
