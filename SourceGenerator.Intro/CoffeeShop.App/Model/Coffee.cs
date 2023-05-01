namespace CoffeeShop.App;

public class Coffee
{
  //public CoffeeTypeEnum CoffeeType { get; set; }
  public bool Decaf { get; set; }
}

public enum CoffeeTypeEnum
{
  EXPRESSO,
  AMERICANO,
  MACCHIATO,
  LATTE,
  CAPPUCCINO,
  MOCHA
}