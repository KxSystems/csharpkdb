  
#!/bin/sh
if [ "$TRAVIS_PULL_REQUEST" = "false" ]; then
    echo "Sonar requires updated Java for Ubuntu 16.04.7, updating repos"
    sudo apt-get update
    sudo apt-get install -y software-properties-common
    sudo add-apt-repository -y ppa:linuxuprising/java
    sudo apt-get update
    echo "Set properties to agree to Java17 license prompts"
    sudo echo oracle-java17-installer shared/accepted-oracle-license-v1-3 select true | /usr/bin/debconf-set-selections
    echo "Install Java17"
    sudo apt install -y oracle-java17-installer --install-recommends
fi
echo "Starting sonar install..."
wget -O sonar.zip https://github.com/SonarSource/sonar-scanner-msbuild/releases/download/5.0.4.24009/sonar-scanner-msbuild-5.0.4.24009-netcoreapp3.0.zip
echo "Unzipping..."
unzip -qq sonar.zip -d tools/sonar
echo "Displaying file structure..."
find .
ls -l tools/sonar
echo "Changing permissions..."
chmod +x tools/sonar/sonar-scanner-4.4.0.2170/bin/sonar-scanner
