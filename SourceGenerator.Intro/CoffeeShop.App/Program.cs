// See https://aka.ms/new-console-template for more information
using CoffeeShop.App.Model;

Coffee coffee = new Coffee
{
    CoffeeType = CoffeeTypeEnum.AMERICANO,
};
Console.WriteLine(coffee.ToString());
