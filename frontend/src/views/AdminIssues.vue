<template>
    <AdminEntitiesNavbar :baseUrl="baseUrlAdmin" />
    <h1>Issues</h1>
    <div class="container" v-for="issue in issues" :key="issue.issueId">
        <div>
            <div class="comment">
                <div class="issueInfo">
                    <div>
                        Issued at:&nbsp;&nbsp;
                        {{ dateFormat(new Date(issue.createdDateTime)) }}h
                    </div>
                    <div>Issue from:&nbsp;&nbsp; {{ issue.userEmail }}</div>
                    <div>
                        Issue for:&nbsp;&nbsp; {{ issue.serviceOwnerEmail }}
                    </div>
                </div>
                <div class="issueContent">
                    {{ issue.reason }}
                </div>
            </div>
        </div>
        <div class="row">
            <textarea
                type="text"
                class="input"
                placeholder="Respond"
                v-model="issue.response"
            ></textarea>
            <button
                class="primaryContained"
                type="submit"
                @click="onRespondToIssue(issue)"
            >
                Respond
            </button>
        </div>
    </div>
</template>

<script>
import AdminEntitiesNavbar from "@/components/AdminEntitiesNavbar.vue";
import axios from "../api/api.js";
import moment from "moment";

export default {
    name: "AdminIssues",
    components: {
        AdminEntitiesNavbar,
    },
    mounted() {
        this.loadIssues();
    },
    methods: {
        loadIssues() {
            axios.get("/api/Admin/GetUnapprovedIssues").then(({ data }) => {
                this.issues = data;
                console.log(data);
            });
        },
        onRespondToIssue(issue) {
            axios.put("/api/Admin/RespondToIssue", issue).then(() => {
                this.loadIssues();
                this.$swal.fire("Response sent to coresponding emails.");
            });
        },
        dateFormat(value) {
            return moment(value).format("YYYY-MM-DD HH:mm");
        },
    },
    data() {
        return {
            baseUrlAdmin: "/admin/",
            issues: [],
        };
    },
};
</script>

<style scoped>
.container {
    display: flex;
    flex-direction: column;
    max-width: 90%;
    padding: 50px;
    background: #fff;
    border-bottom: 1px solid #777676;
    border-radius: 8px;
    padding: 20px;
}

h1 {
    display: flex;
    justify-content: flex-start;
    padding: 20px;
}

.issueInfo {
    display: flex;
    flex-direction: column;
    justify-content: center;
    padding-left: 20px;
    align-items: flex-start;
    min-width: 40%;
    border-right: 1px solid #777676;
}

.issueContent {
    display: flex;
    justify-content: flex-start;
    align-items: flex-start;
    padding: 20px;
}

.input {
    height: 100px;
}

.row {
    display: flex;
    justify-content: flex-end;
    align-items: flex-end;
}
.comment {
    display: flex;
    justify-content: space-between;
    padding-left: 10px;
    border-radius: 5px;
    border: 2px solid #000000;
    height: 100;
}
.container textarea {
    width: 50%;
    padding: 5px 10px;
    border-radius: 5px;
    border: 2px solid #000000;
    margin-top: 15px;
}
button.primaryContained {
    background: #000000;
    color: rgb(255, 255, 255);
    padding: 10px 10px;
    border: none;
    margin-top: 10px;
    margin-left: 10px;
    cursor: pointer;
    border-radius: 5px;
    height: 70%;
}
button.primaryContained:hover {
    background: #193649;
}
</style>
