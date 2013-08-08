# Environmental Control System

## Atmosphere

A collection of gases in a specific volume, with some quantity of heat. The gases in an atmosphere are isothermal, isobaric, and homogeneous. Gas and heat can be added and removed from an atmosphere at will, and derived properties such as temperature and pressure can be easily calculated.

## Mass Cyclometer

Takes an input gas packet, and separates it into Nitrogen (N), Oxygen (O), Carbon Dioxide (C), and toxins (T). The efficiency e of the Cyclometer is an indication of how pure these separate gases are. With more supplied power, the Cyclometer becomes more efficient. The device has a maximum efficiency of 99.99% at full power, and a minimum efficiency of 60% before shutdown.

## Gas Tank

A device which holds pressurized gases. The inside of a gas tank contains an atmosphere, and the tank has a maximum pressure capacity.

If a tank has a pressure higher than its maximum pressure capacity, it will release gases into the surrounding atmosphere until it reaches a nominal level. If the tank's pressure somehow reaches double the maximum pressure capacity, it will explode, causing significant damage to the surrounding area.

Gas tanks can be damaged, with varying levels of effects. Damage to the body of the tank lowers the maximum pressure capacity, while damage to the valves of the tank will cause it to release any gases inside it.

A gas tank can verify the purity of gases in its atmosphere. If the tank contains more than .1% contaminants, it is considered impure. The tank will function as normal, but the mixture of gases inside the tank cannot be verified.

## Gas Tank Array

A device which links together several gas tanks, so that they function as one.

When filling the tanks, the array starts with the lowest-indexed tanks, filling each one to its capacity until all gases have been deposited. Any remaining gases are either vented to space or returned to the system, depending on the system's settings.

When emptying the tanks, the array starts with the highest-indexed tanks, taking as much gas as possible from each one until the desired amount has been taken. If the requested amount of gas exceeds the capacity of all of the tanks, the system will be completely emptied.

The gas tank array can automatically ensure that the tanks are kept at maximum safe capacity by shuffling gases between the tanks. Excess gases from all tanks are collected, and a normal fill cycle is performed. Any extra gas is either vented to space or to an atmosphere, depending on the system settings.

## Room Controller

A device which controls how the ECS interacts with a specific atmosphere attached to the system. It also calculates the nominal values for its specified atmosphere. The ECS uses the information from the room controller to determine what steps are necessary, including how much gas to deposit or withdraw from the tank arrays.