(function (module) {

    var editConfirm = function ($modal) {
        return function (word) {
            var options = {
                templateUrl: "/App/Word/templates/wordform.html",
                controller: function() {
                    this.word = word;
                },  
                controllerAs: "model"
            };

            return $modal.open(options).result;
        };
    };

    module.factory("editConfirm", editConfirm);

}(angular.module("learningWords")));
