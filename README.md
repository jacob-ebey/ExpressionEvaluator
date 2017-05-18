## ExpressionEvaluator

A simple to use, yet powerful expression evaluation framework.

### Overview
**ExpressionEvaluator** exposes the ability to easily build dynamic system configurations.

Let's use the example of building a dynamic configuration for an IoT device to set pin 2 to the value of pin 1.

The first thing to do is to expose a function that set's a pin's value:
``` CSharp
[Function]
static void SetPin(int pin, GpioPinValue value)
{
    _pins[pin].Write(value);
}
```

Next we expose a function to get a pin's value:
``` CSharp
[Function]
static GpioPinValue GetPin(int pin)
{
    return _pins[pin].Read();
}
```

Now for the expression that pulls the functionality together:
``` JSON
{
  "Function": "SetPin",
  "Params": [
    { "Literal": 2 },
    {
      "Function": "GetPin",
      "Params": [
        { "Literal": 1 }
      ]
    }
  ]
}
```