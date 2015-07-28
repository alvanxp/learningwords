(function(module){

    var googleTranslator = function ($http) {
        var translateWord = function (word) {
            $http.get("")
                .then(function (response) {
                    return response.data;
                });
        };



        return {
            translate: translateWord

        };
    };

    module.factory("googleTranslator");

}(angular.module("learningWords")));