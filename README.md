# dot-net-5-api

1. Create local network
docker network create net5tutorial
2. Run mongodb container
docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db -e MONGO_INITDB_ROOT_USERNAME=mongoadmin -e MONGO_INITDB_ROOT_PASSWORD=Pass#word1  --network=networkName mongo
3. Build catalog image
docker build -t catalog:v1 .
4. Run catalog API container
docker run -it --rm -p 5000:80 -e MongoDbSettings:Host=mongo -e MongoDbSettings:Password=Pass#word1 --network=net5tutorial catalog:v1


In order to run kubernetes
5. kubectl create secret generic catalog-secrets --from-literal=mongodb-password='Pass#word1'
6. kubectl apply -f .\catalog.yml
7. kubectl apply -f .\mongodb.yml