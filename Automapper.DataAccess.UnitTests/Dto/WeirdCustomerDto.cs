﻿namespace Automapper.DataAccess.UnitTests.Dto
{
    public class WeirdCustomerDto
    {
        public int C_Id { get; set; }
        public string C_Name { get; set; }
        public string Address { get; set; }
        public int NumOfCars { get; set; }
        public int BlahBlahBlahBlahBlah { get; set; }

        public WeirdLocationDto Location { get; set; }
    }
}