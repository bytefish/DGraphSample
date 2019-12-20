# METAR / ASOS / AWOS Stations #

[Greg Thompson] of the [National Center For Atmospheric Research] maintains a list of weather stations:

* https://www.aviationweather.gov/docs/metar/stations.txt

I have used the file to match stations to the IATA (FAA) ID, so I know where exactely the 
stations are located and to get more informations about stations. The file was preprocessed, 
so that headers are ignored by the parser. 

This was done by simply adding a comment character ``!`` to the header, so for example:

```
ETHIOPIA           27-DEC-01
```

... will become:

```
!ETHIOPIA           27-DEC-01
```

... and thus is ignored by the parser.

[National Center For Atmospheric Research]: https://ncar.ucar.edu/
[Greg Thompson]: https://ral.ucar.edu/~gthompsn/