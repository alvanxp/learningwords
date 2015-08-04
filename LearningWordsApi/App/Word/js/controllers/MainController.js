(function (module) {
    var MainController = function ($scope, $http,editConfirm, alerting) {
        var addNewWord = function () {

            var newWord = {
                Word: $scope.word,
                Language: 'EN',
                Description: $scope.description,
                ToWord: $scope.toword,
                ToLanguage: 'ES',
                ToDescription: $scope.todescription
            };

            $http.post('api/words/', newWord)
                .success(function (data, status, headers, config) {
                    alerting.addInfo("Word Added: " + $scope.word)
                    clear();
                })
                .error(function (data, status, headers, config) {
                    alerting.addInfo("Error");
                });
        };

        var updateWord = function () {

            var updatedWord = {
                WordId: $scope.wordId,
                Word: $scope.word,
                Language: 'EN',
                Description: $scope.description,
                ToWord: $scope.toword,
                ToLanguage: 'ES',
                ToDescription: $scope.todescription
            };

            $http.put('api/words/', updatedWord)
                .success(function (data, status, headers, config) {
                    alerting.addInfo("Word updated: " + $scope.word)
                    clear();
                })
                .error(function (data, status, headers, config) {
                    alerting.addInfo("Error");
                });        
        };

        var saveword = function () {
          
                if ($scope.isEditing) {
                    updateWord();
                }
                else {
                    addNewWord();
                }
           
            // $http.get("http://learningwords.azurewebsites.net/api/words/random?from=EN&to=ES").then()
            
        };

        var getWords = function(){
            $http.get('api/words/?language=EN&toLanguage=ES')
                .then(function (response) {
                    $scope.words = response.data;
                });
        };

        var clear = function () {
            $scope.word = '';
            $scope.description = '';
            $scope.toword = '';
            $scope.todescription = '';
            $scope.allowEdition = false;
        };

        var newWord = function () {
            $scope.allowEdition = true;
            $scope.isEditing = false;
            $scope.message = 'Enter you new word!';
            showPopUp();
        };

        var editWord = function (word) {
            $scope.isEditing = true;
            $scope.allowEdition = true;
            $scope.wordId = word.WordId;
            $scope.word = word.Word;
            $scope.description = word.Description;
            $scope.toword = word.ToWord;
            $scope.todescription = word.ToDescription;
            $scope.message = '';
            showPopUp();
        };


        var showPopUp = function () {
            editConfirm($scope).then(function (result) {
                $scope = result;
                saveword();
            }, function () {
                clear();
            });
        };

        

        $scope.wordId='';
        $scope.word = '';
        $scope.description = '';
        $scope.toword = '';
        $scope.todescription = '';
        $scope.saveWord = saveword;
        $scope.clear = clear;
        $scope.words = [];
        $scope.allowEdition = false;
        $scope.newWord = newWord;
        $scope.editWord = editWord;
        $scope.isEditing = false;
        $scope.message = '';
        getWords();
    };
    module.controller("MainController", MainController)
}(angular.module("learningWords")));