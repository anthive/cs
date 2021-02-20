# AntHive.IO sample bot in *C#*

## [Import](https://github.com/new/import) this sample bot.

## Requirements
- Set your username in [ANTHIVE](ANTHIVE) file.
- Push to your Github account.
- Do not push your code to sample bot repo.
- Register your bot at https://profile.anthive.io/bots

## Debug and Sandbox
- git push origin master
- Verify/Compile your latest commit in **settings** page https://profile.anthive.io/settings/
- Start new game at https://profile.anthive.io/new-game/
- Replay game step by step
- View logs

### Run locally (not required)
```
dotnet run Bot.cs
```
It will start localhost server on port :7070 **Do not change port**

```
curl -X 'POST' -d @payload.json http://localhost:7070
```

### [Rules](https://anthive.io/rules/) and [Leaderboard](https://anthive.io/leaderboard/)