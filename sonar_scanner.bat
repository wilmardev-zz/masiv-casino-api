dotnet test 5.Test/Masiv.Casino.Test/Masiv.Casino.Test.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
dotnet build-server shutdown
dotnet sonarscanner begin /k:"masiv-casino-api" \
                          /n:"masiv-casino-api" \
                          /o:"wsduque-repos" \
                          /v:latest \
                          /d:sonar.host.url="https://sonarcloud.io" \
                          /d:sonar.login="f7f8fe60cea48f9d528cc28df702e0d72269c09f" \
                          /d:sonar.verbose=true \
                          /d:sonar.cs.opencover.reportsPaths="5.Test/Masiv.Casino.Test/coverage.opencover.xml" \
                          /d:sonar.exclusions=**/bin/**/*,**/obj/**/*,5.Test/Masiv.Casino.Test/** \
                          /d:sonar.coverage.exclusions=5.Test/Masiv.Casino.Test/** \
                          /d:sonar.branch.name="master"
dotnet build Masiv.Casino.sln
dotnet sonarscanner end /d:sonar.login="f7f8fe60cea48f9d528cc28df702e0d72269c09f"
pause
