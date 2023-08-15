# CalendarAPI
Simple Calendar API backend for practicing frontend skills

# How to start

`docker-compose up -d` - it will start up a local mongo and api instance, populate mongo with some initial data.

# General

To access swagger: `http://localhost:5000/swagger/index.html`
Mongo Ids are all bson object ids (e.g. `64cff2a8de7286f37fcd0799`).

This is a simple API to create a calendar with events for a gym. Think - schedule of group training sessions, letting people register, etc.

# Why Mongo?

At first requirements were much simpler. Later on they creeped up and now mongo doesn't make much sense here.
But I also kinda want to mess with mongo a bit more, so why not shoe-horn it here.

# Future Updates?

Will tidy up the code once I get some extra time.
Also, will resolve any bugs that are found, etc. - raise an issue.

Will add auth later on.