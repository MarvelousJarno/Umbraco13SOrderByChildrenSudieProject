angular.module("umbraco").controller("CustomPrevalueEditors.MultiValuesController",
    function ($scope, $timeout) {

        //NOTE: We need to make each item an object, not just a string because you cannot 2-way bind to a primitive.

        $scope.newItem = "";
        $scope.orderBy = "";
        $scope.hasErrorText = false;
        $scope.hasErrorOrderBy = false;
        $scope.focusOnNew = false;

        if (!Utilities.isArray($scope.model.value)) {
            //make an array from the dictionary
            var items = [
                {
                    value: "Name",
                    sortOrder: 0,
                    orderBy: "ASC",
                    id: generateUUID()
                },
                {
                    value: "CreateDate",
                    sortOrder: 1,
                    orderBy: "DESC",
                    id: generateUUID()
                },
                {
                    value: "PublishDate",
                    sortOrder: 2,
                    orderBy: "DESC",
                    id: generateUUID()
                }
            ];

            var index = 3;

            for (var i in $scope.model.value) {
                items.push({
                    value: $scope.model.value[index].value,
                    sortOrder: $scope.model.value[index].sortOrder,
                    orderBy: $scope.orderBy,
                    id: generateUUID()
                });
                index++;
            }
            //ensure the items are sorted by the provided sort order
            items.sort(function (a, b) { return (a.sortOrder > b.sortOrder) ? 1 : ((b.sortOrder > a.sortOrder) ? -1 : 0); });

            //now make the editor model the array
            $scope.model.value = items;
        }

        $scope.remove = function (item, evt) {
            evt.preventDefault();

            $scope.model.value = _.reject($scope.model.value, function (x) {
                return x.value === item.value;
            });

        };

        $scope.add = function (evt) {
            evt.preventDefault();
            if ($scope.newItem && $scope.orderBy) {
                if (!_.contains($scope.model.value, $scope.newItem)) {
                    $scope.model.value.push({ value: $scope.newItem, orderBy: $scope.orderBy, id: generateUUID()  });
                    $scope.newItem = "";
                    $scope.orderBy = "";
                    $scope.hasErrorText = false;
                    $scope.hasErrorOrderBy = false;
                    $scope.focusOnNew = true;
                    return;
                }
            }

            //there was an error, do the highlight (will be set back by the directive)
            if ($scope.newItem) {
                $scope.hasErrorText = false;
            } else
            {
                $scope.hasErrorText = true;
            }

            if ($scope.orderBy) {
                $scope.hasErrorOrderBy = false;
            } else {
                $scope.hasErrorOrderBy = true;
            }
        };

        $scope.sortableOptions = {
            axis: 'y',
            containment: 'parent',
            cursor: 'move',
            items: '> div.control-group',
            tolerance: 'pointer',
            update: function (e, ui) {
                // Get the new and old index for the moved element (using the text as the identifier, so 
                // we'd have a problem if two prevalues were the same, but that would be unlikely)
                var newIndex = ui.item.index();
                var movedPrevalueText = $('input[type="text"]', ui.item).val();
                var originalIndex = getElementIndexByPrevalueText(movedPrevalueText);

                // Move the element in the model
                if (originalIndex > -1) {
                    var movedElement = $scope.model.value[originalIndex];
                    $scope.model.value.splice(originalIndex, 1);
                    $scope.model.value.splice(newIndex, 0, movedElement);
                }
            }
        };

        $scope.createNew = function (event) {
            if (event.keyCode == 13) {
                $scope.add(event);
            }
        };

        function getElementIndexByPrevalueText(value) {
            for (var i = 0; i < $scope.model.value.length; i++) {
                if ($scope.model.value[i].value === value) {
                    return i;
                }
            }

            return -1;
        }

        function generateUUID() { // Public Domain/MIT
            var d = new Date().getTime();//Timestamp
            var d2 = ((typeof performance !== 'undefined') && performance.now && (performance.now() * 1000)) || 0;//Time in microseconds since page-load or 0 if unsupported
            return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                var r = Math.random() * 16;//random number between 0 and 16
                if (d > 0) {//Use timestamp until depleted
                    r = (d + r) % 16 | 0;
                    d = Math.floor(d / 16);
                } else {//Use microseconds since page-load if supported
                    r = (d2 + r) % 16 | 0;
                    d2 = Math.floor(d2 / 16);
                }
                return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16);
            });
        }

    });