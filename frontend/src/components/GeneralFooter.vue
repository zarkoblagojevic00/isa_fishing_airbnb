<template>
    <div class="footer">
        <div class="content">
            <div class="logo"></div>

            <div class="menus">
                <div class="submenu">
                    General
                    <div class="options">
                        <div class="option" @click="$router.push('/')">
                            Home page
                        </div>
                        <div class="option" @click="$router.push('/villas')">
                            Villas
                        </div>
                        <div class="option" @click="$router.push('/boats')">
                            Boats
                        </div>
                        <div
                            class="option"
                            @click="$router.push('/adventures')"
                        >
                            Adventures
                        </div>
                    </div>
                </div>
                <div class="submenu">
                    account
                    <div class="options">
                        <div class="option" @click="GoToLogin()">login</div>
                    </div>
                </div>
                <div class="submenu">
                    contact
                    <div class="options">
                        <div class="contact-info-item">zamisr@gmail.com</div>
                        <div class="contact-info-item">+381652616432</div>
                        <div class="contact-info-item">
                            Trg Dositeja Obradovića, 6
                        </div>
                        <div class="contact-info-item">Novi Sad, Serbia</div>
                    </div>
                </div>
            </div>
        </div>
        <div class="bottom-line">Copyright &copy; 2021. Zamisr</div>
    </div>
</template>

<script>
export default {
    name: "GeneralFooter",
    data() {
        return {};
    },
    props: {},
    methods: {
        GoToLogin() {
            window.location.href = "/login";
        },
    },
};
</script>

<style scoped>
.footer {
    min-height: 300px;
    width: 100%;
    background-color: rgb(74, 88, 89);
    position: relative;
    display: flex;
    flex-direction: column;
    justify-content: space-between;
}

.content {
    display: flex;
    padding: 10px 40px;
    width: 100% - 40px;
    height: 100%;
    justify-content: space-between;
    flex-direction: row;
}
.bottom-line {
    height: 50px;
    width: 100% - 40px;
    background-color: rgb(50, 55, 59);
    padding: 10px;
    font-size: 13px;
    display: flex;
    justify-content: center;
    align-items: center;
    color: #aaa;
    font-weight: 550;
    padding-left: 40px;
}

.logo {
    width: 200px;
    height: 200px;
    background-image: url("../assets/zamisr-logo-modified.png");
    background-repeat: no-repeat;
    background-size: cover;
}

.menus {
    display: flex;
    flex-flow: row wrap;
    justify-content: center;
    height: 100%;
    color: rgba(255, 255, 255, 0.8);
}

.submenu {
    width: 200px;
    text-transform: capitalize;
}

.options {
    margin-top: 20px;
    margin-bottom: 20px;
}
.option {
    margin-top: 10px;
    font-size: 13px;
    cursor: pointer;
    text-decoration: underline;
}

.contact-info-item {
    margin-top: 10px;
    font-size: 13px;
    text-transform: none;
}

@media (max-width: 900px) {
    .bottom-line {
        padding-left: 10px;
        justify-content: center;
    }
    .content {
        padding: 10px;
        flex-direction: column-reverse;
        justify-content: center;
        align-items: center;
    }
}
</style>
