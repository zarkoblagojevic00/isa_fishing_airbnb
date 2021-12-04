<template>
	<div class="wrapperdiv">	
        <div class="villa-picker">
            <h3>Pick a villa:</h3>
            <select class="select-villa" v-model="selectedVilla">
                <option disabled value="">Please select one</option>
                <option v-for="villa in allVillas" :key="villa.id" :value="villa.id">{{villa.name}}</option>
            </select>
        </div>
		<div class="calendar-parent">
			<calendar-view
				:items="items"
				:show-date="showDate"
				:time-format-options="{ hour: 'numeric', minute: '2-digit' }"
				:enable-drag-drop="true"
				:disable-past="disablePast"
				:disable-future="disableFuture"
				:show-times="showTimes"
				:date-classes="myDateClasses"
				:display-period-uom="displayPeriodUom"
				:display-period-count="displayPeriodCount"
				:starting-day-of-week="startingDayOfWeek"
				:class="themeClasses"
				:period-changed-callback="periodChanged"
				:current-period-label="useTodayIcons ? 'icons' : ''"
				:displayWeekNumbers="displayWeekNumbers"
				:enable-date-selection="false"
			>
				<template #header="{ headerProps }">
					<calendar-view-header :header-props="headerProps" @input="setShowDate" />
				</template>
			</calendar-view>
		</div>
	</div>
</template>
<script>
import "../../node_modules/vue-simple-calendar/dist/style.css"
import "../../node_modules/vue-simple-calendar/static/css/default.css"
import "../../node_modules/vue-simple-calendar/static/css/holidays-us.css"
import { CalendarView, CalendarViewHeader, CalendarMath } from "vue-simple-calendar" 
export default {
	name: "VillaCalendar",
	components: {
		CalendarView,
		CalendarViewHeader,
	},
	data() {
		return {
			showDate: this.thisMonth(1),
			message: "",
			startingDayOfWeek: 0,
			disablePast: false,
			disableFuture: false,
			displayPeriodUom: "month",
			displayPeriodCount: 1,
			displayWeekNumbers: false,
			showTimes: true,
			newItemTitle: "",
			newItemStartDate: "",
			newItemEndDate: "",
			useDefaultTheme: true,
			useHolidayTheme: true,
			useTodayIcons: false,

            selectedVilla: "",
            allVillas: [],
			items: [],
		}
	},
	computed: {
		userLocale() {
			return CalendarMath.getDefaultBrowserLocale
		},
		dayNames() {
			return CalendarMath.getFormattedWeekdayNames(this.userLocale, "long", 0)
		},
		themeClasses() {
			return {
				"theme-default": this.useDefaultTheme,
				"holiday-us-traditional": this.useHolidayTheme,
				"holiday-us-official": this.useHolidayTheme,
			}
		},
		myDateClasses() {
			const o = {}
			const theFirst = this.thisMonth(1)
			const ides = [2, 4, 6, 9].includes(theFirst.getMonth()) ? 15 : 13
			const idesDate = this.thisMonth(ides)
			o[CalendarMath.isoYearMonthDay(idesDate)] = "ides"
			o[CalendarMath.isoYearMonthDay(this.thisMonth(21))] = ["do-you-remember", "the-21st"]
			return o
		},
	},
	mounted() {
		this.newItemStartDate = CalendarMath.isoYearMonthDay(CalendarMath.today());
		this.newItemEndDate = CalendarMath.isoYearMonthDay(CalendarMath.today());
        this.getAllVillas();
	},
	methods: {
		thisMonth(d, h, m) {
			const t = new Date()
			return new Date(t.getFullYear(), t.getMonth(), d, h || 0, m || 0)
		},
		setShowDate(d) {
			this.message = `Changing calendar view to ${d.toLocaleDateString()}`
			this.showDate = d
		},
        getAllVillas() {
            let vue = this;
            fetch("/api/VillaManagement/GetOwnedVillas", {
                method: 'GET',
                header: {
                    'Content-type': 'application-json',
                    'Set-Cookie': document.cookie
                }
            }).then(response => {
                if (response.status == 200){
                    return response.json();
                }
                else{
                    return response.text();
                }
            }).then(data => {
                var stringConstructor = "test".constructor;
                if (data.constructor == stringConstructor){
                    alert("Something went wrong!\nError message: " + data);
                    return;
                }
                vue.allVillas = new Array();
                for (let villa of data){
                    vue.allVillas.push({
                        id: villa.villaId,
                        name: villa.name
                    });
                }
            })
        },
        RefreshCalendar() {
            let vue = this;
            fetch("/api/GeneralService/GetReservationHistory?serviceId=" + this.selectedVilla, {
                method: 'GET',
                header: {
                    'Content-type' : 'application-json',
                    'Set-Cookie': document.cookie
                }
            }).then(response => {
                if (response.status == 200){
                    return response.json();
                }
                return response.text();
            }).then(data => {
                let strconst = "test".constructor;
                if (data.constructor == strconst){
                    alert("Something went wrong!\nError message: " + data);
                    return;
                }

                vue.items = new Array();
                for (let i = 0; i < data.length; i++){
                    let reservation = data[i].reservation;
                    if (!reservation.isCanceled){
                        let item = {
                            id: i,
                            startDate: reservation.startDateTime.split("T")[0] + " " + reservation.startDateTime.split("T")[1].split(".")[0],
                            endDate: reservation.endDateTime.split("T")[0] + " " + reservation.startDateTime.split("T")[1].split(".")[0],
                        }

                        if (reservation.isServiceUnavailableMarker){
                            item.title = "Service unavailable";
                            item.style = "background-color: #D8DC6A; cursor: pointer";
                        }
                        else if (reservation.isPromo){
                            item.title = "Promo";
                            item.style = "background-color: #1985A1; cursor: pointer";
                        }
                        else {
                            item.title = "Reservation";
                            item.style = "background-color: #EB8258; cursor: pointer";
                        }

                        vue.items.push(item);
                    }
                }
            });
        }
	},
    watch: {
        selectedVilla: function() {
            this.RefreshCalendar();     
        }
    },
}
</script>

<style>
.wrapperdiv {
	display: flex;
    flex-direction: column;
	width: 100%;
	height: 100%;
	margin-left: auto;
	margin-right: auto;
}
.calendar-parent {
	display: flex;
    max-height: 50%;
	flex-direction: column;
	flex-grow: 1;
	overflow-x: hidden;
	overflow-y: hidden;
	background-color: white;
}
.cv-wrapper.period-month.periodCount-2 .cv-week,
.cv-wrapper.period-month.periodCount-3 .cv-week,
.cv-wrapper.period-year .cv-week {
	min-height: 6rem;
}
.theme-default .cv-item.birthday {
	background-color: #e0f0e0;
	border-color: #d7e7d7;
}
.theme-default .cv-item.birthday::before {
	content: "\1F382";
	margin-right: 0.5em;
}
.theme-default .cv-day.ides {
	background-color: #ffe0e0;
}
.ides .cv-day-number::before {
	content: "\271D";
}
.cv-day.do-you-remember.the-21st .cv-day-number::after {
	content: "\1F30D\1F32C\1F525";
}

.villa-picker {
    height: 70px;
    display: flex;
    flex-flow: row wrap;
    align-items: center;
}

.select-villa {
    width: 200px;
    height: 40px;
    margin-left: 10px;
    outline: none;
    border: none;
}
</style>