# Events

Custom EventManager based on c# delegates instead of UnityEvents, created for education purpose. I based the idea on a pseudo-code found on the internet, and some other samples.

## IGameEvent and Event

Every single event is a separate class implementing IGameEvent interface. I'm not sure if the whole idea behind this approach is good. So far I found this system to be extensible and easy to use.

## EventManager

It takes care of delegates for us. No need to create new ones. All we need to do is to create a new event(class). 
