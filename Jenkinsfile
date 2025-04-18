pipeline {
    agent {
        label 'MacBook-Pro'
    }
    stages {
        stage('Git Checkout') {
            steps {
                script {
                    echo "현재 체크아웃된 브랜치: ${env.GIT_BRANCH}"
                }
            }
        }
    }
}
