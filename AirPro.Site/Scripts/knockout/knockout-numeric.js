//Limits the string length in a numeric field
//Usage: self.year = ko.observable().extend({ numeric: 4 });
ko.extenders.numeric = function (target, options) {
    //create a writable computed observable to intercept writes to our observable
    var result = ko.pureComputed({
        read: target,  //always return the original observables value
        write: function (newValue) {
            if (!options) {
                options = {
                    precision: 0
                }
            }

            var current = target(),
                roundingMultiplier = Math.pow(10, options.precision),
                newValueAsNum = isNaN(newValue) ? 0 : +newValue,
                valueToWrite = Math.round(newValueAsNum * roundingMultiplier) / roundingMultiplier;

            if (options.max && valueToWrite > options.max) {
                valueToWrite = options.max;
            }

            //only write if it changed
            if (valueToWrite !== current) {
                target(valueToWrite);
            } else {
                //if the rounded value is the same, but a different value was written, force a notification for the current field
                if (newValue !== current) {
                    target.notifySubscribers(valueToWrite);
                }
            }
        }
    }).extend({ notify: 'always' });

    //initialize with current value to make sure it is rounded appropriately
    result(target());

    //return the new computed observable
    return result;
};