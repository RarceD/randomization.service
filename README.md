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