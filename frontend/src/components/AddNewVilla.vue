<template>
    <div class="content">
        <div class="content-pane">
            <div class="input-wrapper">
                <span class="label">Villa name:</span>
                <input class="input-field" type="text" v-model="name" :class="[ValidateString(name) ? '' : 'error-outline']">
            </div>
            <div class="input-wrapper">
                <span class="label">Price per day:</span>
                <input class="input-field" type="text" v-model="pricePerDay" v-on:keypress="isNumber($event)" :class="[ValidatePrice() ? '' : 'error-outline']">
            </div>
            <div class="input-wrapper">
                <span class="label">Address:</span>
                <input class="input-field" type="text" v-model="address" :class="[ValidateString(address) ? '' : 'error-outline']">
            </div>
            <div class="input-wrapper">
                <span class="label">Longitude:</span>
                <input class="input-field" type="number" min="-180" max="80"  v-model="longitude" :class="[ValidateLongitude() ? '' : 'error-outline']">
            </div>
            <div class="input-wrapper">
                <span class="label">Latitude:</span>
                <input class="input-field" type="number" min="-90" max="90"  v-model="latitude" :class="[ValidateLatitude() ? '' : 'error-outline']">
            </div>
            <div class="input-wrapper">
                <span class="label">Capacity:</span>
                <input class="input-field" type="number" min="0" v-model="capacity" :class="[ValidateNumber(capacity) ? '' : 'error-outline']">
            </div>
            <div class="horizontal-wrapper">
                <input type="checkbox" class="input-checkbox" v-model="percentageTakenFromCancelation">
                <span class="label">Charge canceled reservations?</span>
            </div>
            <div class="input-wrapper">
                <span class="label">Percentage to take:</span>
                <input class="input-field" type="number" min="0" max="100" v-model="percentageToTake" :disabled="percentageTakenFromCancelation == false" :class="[ValidatePercentage(capacity) ? '' : 'error-outline']">
            </div>
            <div class="input-wrapper">
                <span class="label">Number of rooms:</span>
                <input class="input-field" type="number" min="0" max="100" v-model="numberOfRooms" :class="[ValidateNumber(numberOfRooms) ? '' : 'error-outline']">
            </div>
            <div class="input-wrapper">
                <span class="label">Number of beds:</span>
                <input class="input-field" type="number" min="0" max="100" v-model="numberOfBeds" :class="[ValidateNumber(numberOfBeds) ? '' : 'error-outline']">
            </div>
        </div>
        <div class="content-pane">
            <div class="input-wrapper">
                <span class="label">Promo description:</span>
                <textarea class="input-textarea" type="text" v-model="promoDescription" :class="[ValidateString(promoDescription) ? '' : 'error-outline']"></textarea>
            </div>
            <div class="input-wrapper">
                <span class="label">Terms of use:</span>
                <textarea class="input-textarea" type="text" v-model="termsOfUse" placeholder="Not required"></textarea>
            </div>
            <div class="input-wrapper">
                <span class="label">Additional equipment:</span>
                <textarea class="input-textarea" type="text" v-model="additionalEquipment" placeholder="Not required"></textarea>
            </div>
            <div class="input-wrapper" v-if="errors.length > 0">
                <span class="label red">Errors:</span>
                <ul>
                    <li class="red" v-for="error in errors" :key="error">*{{error}}</li>
                </ul>
            </div>
        </div>
    </div>
    <div class="submit-div">
        <button class="submit-btn" @click="SendRequest()">Create Villa!</button>
    </div>
</template>

<script>

export default {
    name: "AndNewVilla",
    data() {
        return {
            name: "",
            pricePerDay: "",
            address: "",
            longitude: 0,
            latitude: 0,
            promoDescription: "",
            termsOfUse: "",
            additionalEquipment: "",
            availableFrom: null,
            availableTo: null,
            capacity: 0,
            percentageTakenFromCancelation: false,
            percentageToTake: 0,
            numberOfBeds: 0,
            numberOfRooms: 0,
            errors : []
        }
    },
    methods: {
        isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if ((charCode > 31 && (charCode < 48 || charCode > 57)) && charCode !== 46) {
                evt.preventDefault();;
            } else {
                return true;
            }
        },
        ValidateString(str) {
            if (str == 0)
                return false;
            return true;
        },
        ValidatePrice() {
            if (this.pricePerDay.length == 0)
                return false;

            let input = this.pricePerDay.trim();
            input = input.split(" ");

            if (input.length > 1)
                return false;
            
            input = input[0];

            input = parseFloat(input);
            if (input == undefined || input == null)
                return false;
            
            if (input < 0)
                return false;

            return true;
        },
        ValidateLongitude(){
            if (this.longitude > 80 || this.longitude < -180)
                return false;

            return true;
        },
        ValidateLatitude(){
            if (this.latitude > 90 || this.latitude < -90)
                return false;

            return true;
        },
        ValidateNumber(num) {         
            if (num < 1 || !Number.isInteger(num))
                return false;

            return true;
        },
        ValidatePercentage(){
            if (!this.percentageTakenFromCancelation)
                return true;
            
            if (this.percentageToTake <= 100 && this.percentageToTake > 0)
                return true;

            return false;
        },
        SendRequest() {
            this.errors = new Array();

            if (!this.ValidateString(this.name) ||
                !this.ValidatePrice() ||
                !this.ValidateString(this.address) ||
                !this.ValidateLongitude() ||
                !this.ValidateLatitude() ||
                !this.ValidateNumber(this.capacity) ||
                !this.ValidatePercentage() ||
                !this.ValidateNumber(this.numberOfBeds) ||
                !this.ValidateNumber(this.numberOfRooms) ||
                !this.ValidateString(this.promoDescription)){
                    this.errors.push('All fields marked with red outline need to be corrected!');
                    return false;
                }
            
            let dto = {
                Name: this.name,
                PricePerDay: this.pricePerDay,
                Address: this.address,
                Longitude: this.longitude,
                Latitude: this.latitude,
                PromoDescription: this.promoDescription,
                TermsOfUser: this.termsOfUse,
                AdditionalEquipment: this.additionalEquipment,
                Capacity: this.capacity,
                IsPercentageTakenFromCanceledReservations: this.percentageTakenFromCancelation,
                PercentageToTake: this.percentageToTake,
                NumberOfBeds: this.numberOfBeds,
                NumberOfRooms: this.numberOfRooms
            }

            let vue = this;
            fetch(process.env.VUE_APP_BASE_API_URL + "api/VillaManagement/CreateVilla", {
                method: 'POST',
                redirect: 'follow',
                headers: {
                    'Content-type': 'application/json',
                    'Set-Cookie': document.cookie
                },
                body: JSON.stringify(dto),
            }).then(response => {
                if (response.status != 200){
                    alert("Something went wrong!\nStatus code: " + response.status);
                    return;
                }
                alert('Success! New villa has been created!');
                return response.json();
            }).catch(data => {
                vue.errors = new Array();
                if (data.errors.length > 0){
                    for (let error of data.errors){
                        vue.errors.push(data.errors[error]);
                    }
                }
            });


            return true;
        }
    },
}
</script>

<style scoped>

.content {
    min-height: calc(100% - 75px);
    display: flex;
    flex-flow: row wrap;
    position: relative;
}

.content-pane {
    display: flex;
    flex-direction: column;
    height: auto;
    width: 50%;
    min-width: 145px;
    padding-left: 10px;
    padding-right: 10px;
    box-sizing: border-box;
}

.input-wrapper {
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    margin-bottom: 15px;
}

.label {
    font-size: 14px;
    color: black;
    margin-bottom: 5px;
    padding-left: 10px;
}

.input-field {
    height: 50px;
    border-radius: 15px;
    outline: none;
    border: 1px solid #c3c3c3;
    max-width: 400px;
    min-width: 200px;
    width: 100%;
    padding-left: 15px;
    padding-right: 15px;
    box-sizing: border-box;
    font-size: 15px;
}

.input-textarea {
    height: 150px;
    max-width: 400px;
    min-width: 200px;
    width: 100%;
    border: 1px solid #c3c3c3;
    border-radius: 15px;
    padding: 15px;
    box-sizing: border-box;
    resize: none;
    font-size: 15px;
    outline: none;
}

.horizontal-wrapper {
    max-width: 400px;
    min-width: 200px;
    width: 100%;
    display: flex;
    margin-bottom: 10px;
    padding-left: 10px;
}

.input-checkbox {
    width: 15px;
    height: 15px;
    position: relative;
    top: -2px;
}

.submit-btn {
    height: 50px;
    border-radius: 15px;
    border: none;
    font-size: 20px;
    color: white;
    background-color: #345fed;
    cursor: pointer;
    font-size: 18px;
    transition: background-color 300ms linear;
}

.submit-btn:hover {
  background-color: #54cc39;
}

.red {
    color: red !important;
}

li {
    display: flex; 
    color: red;
    font-size: 13px;
}

.error-outline {
    border: 1px solid red !important;
}
</style>