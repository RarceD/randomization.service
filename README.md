# RandoMice Service

This is a modern version of the original [RandoMice](https://github.com/RvE54/RandoMice) tool. You can randomize your animals via API REST request like this:
```json
{
    "numberOfGroups": 5,
    "numberoOfAnimalsPerGroup": 5,
    "animalMeasurements" : [
        {
            "animalID": "Animal 1",
            "value": 12.23
        },
        {
            "animalID": "Animal 2",
            "value": 34.53
        }
    ]
}
```

## Run with docker

```sh
docker build -t randomization .
docker run --name randomization -p 5000:5000 randomization
```

Check if the app is running ok:

```sh
curl localhost:5000/alive
```

See the logs:

```sh
docker exec -it randomization bash
```

## License

This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version - see the LICENSE file for details.