# CUIT Service

It will return information based on a provided CUIT number.

## Continuous Deployment to test and production environments

We are following the same criteria than [Doppler
Forms](https://github.com/MakingSense/doppler-forms/blob/master/README.md#continuous-deployment-to-test-and-production-environments).

## Secrets

In the long term, the idea is to remove configuration from this repository and moving it to [Doppler
Swarm](https://github.com/MakingSense/doppler-swarm), but by the moment, in order to develop and test
it easily we are incluiding the encrypted secrets in this repository.

To learm how to deal with our SOPS encrypted files, please read [Doppler
Swarm](https://github.com/MakingSense/doppler-swarm/blob/master/README.md#secrets).

## To do

- [x] EditorConfig, Prettier, etc
- [x] Continuous Deployment (Jenkins, DockerHub)
- [x] Define resources (URLs, schema, etc)
- [x] Expose hardcoded information (it could be also useful in test environments)
- [ ] Validate JWT token
- [ ] Rate limit for normal users, unlimited for Doppler SU
- [ ] Connect with our backend service
- [ ] Deploy to our Swarm
- [ ] Keep our secrets and configurations in encrypted files here or in our
      Swarm repository
