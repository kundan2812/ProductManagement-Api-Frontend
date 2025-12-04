var app = angular.module("productApp", []);

app.controller("productCtrl", function ($scope, $http) {

    const baseUrl = "https://localhost:7128/api/Items";
    $scope.items = [];
    $scope.showForm = false;
    $scope.isEdit = false;
    $scope.form = {};
    $scope.message = "";
    $scope.success = true;
    $scope.placeholder = "placeholder.png";

    $scope.init = function () {
        $scope.loadItems();
    };

    $scope.loadItems = function () {
        $http.get(baseUrl + "/ListItems")
            .then(function (response) {
                $scope.items = response.data;
            }, function () {
                $scope.showMessage("Error loading items", false);
            });
    };

    $scope.showAddForm = function () {
        $scope.showForm = true;
        $scope.isEdit = false;
        $scope.form = {};
        $scope.clearMessage();
    };

    $scope.editItem = function (item) {
        $scope.showForm = true;
        $scope.isEdit = true;
        $scope.form = angular.copy(item);
        $scope.clearMessage();
    };

    $scope.deleteItem = function (code) {
        if (!confirm("Delete this item?")) return;

        $http.delete(baseUrl + "/DeleteItem/" + code)
            .then(() => {
                $scope.showMessage("Item deleted successfully", true);
                $scope.loadItems();
            }, () => $scope.showMessage("Error deleting item", false));
    };
    $scope.saveItem = function () {
        if (!$scope.form.itemCode || !$scope.form.description ||
            !$scope.form.sellingPrice || !$scope.form.costPrice) {
            $scope.showMessage("Please fill all required fields!", false);
            return;
        }

        if ($scope.form.photoFile) {
            const file = $scope.form.photoFile;
            if (!file.type.startsWith("image/")) {
                $scope.showMessage("Only image files allowed!", false);
                return;
            }
            if (file.size > 3 * 1024 * 1024) {
                $scope.showMessage("Image size must be less than 2MB!", false);
                return;
            }
        }
        if ($scope.isEdit) {
            $http.put(baseUrl + "/EditItem", $scope.form)
                .then(() => {
                    $scope.showMessage("Item updated successfully!", true);
                    $scope.loadItems();
                    $scope.cancelForm();
                }, () => $scope.showMessage("Error updating item", false));
        } else {
            $http.post(baseUrl + "/AddItem", $scope.form)
                .then(() => {
                    $scope.showMessage("Item added successfully!", true);
                    $scope.loadItems();
                    $scope.cancelForm();
                }, () => $scope.showMessage("Error adding item", false));
        }
    };
    $scope.cancelForm = function () {
        $scope.showForm = false;
        $scope.form = {};
        $scope.clearMessage();
    };
    $scope.onFileChange = function (element) {
        const file = element.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function (e) {
                $scope.$apply(function () {
                    $scope.form.photoBase64 = e.target.result.split(",")[1]; 
                    $scope.form.photoFile = file; 
                });
            };
            reader.readAsDataURL(file);
        }
    };
    $scope.showMessage = function (msg, isSuccess) {
        $scope.message = msg;
        $scope.success = isSuccess;
        setTimeout(() => {
            $scope.$apply(function () { $scope.message = ""; });
        }, 4000);
    };

    $scope.clearMessage = function () {
        $scope.message = "";
    };

    $scope.init();
});
