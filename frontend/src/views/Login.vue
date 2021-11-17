<template>
    <div class="whole-page"> 
        <div class="backdrop">
            <div class="content">
                <div class="card">
                    <div class="card-content">
                        <div class="welcome">
                            Welcome!
                        </div>
                        <div class="wrapper-input">     
                            <input type="text" class="input-field input-username" placeholder="Username" v-model="username">
                            <input type="password" class="input-field input-password" placeholder="Password" v-model="password">
                        </div>

                        <span class="error">{{error != '' ? 'Error: ' + error : ''}}</span>

                        <div class="login-forgot">
                            <a href="#">Forgot password?</a>
                            <button class="login-btn">
                                Login
                            </button>
                        </div>

                        <hr class="hr">

                        <div class="card-footer">
                            <span class="footer-text">
                                Don't have an account? <br>
                                No worries!
                                Register <a href="#">here</a>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>

export default {
    name: "Login",
    components: {},
    data() {
        return {
            username : "",
            password : "",
            error : "",
        }
    }
}

</script>

<style scoped>
.whole-page {
    min-width: 100%;
    min-height: 100vh;
    background-image: url("../assets/gallery.jpeg");
    background-repeat: no-repeat;
    background-size: cover;
    position: relative;
    overflow: hidden;
    font-size: 13px;
}

.backdrop {
    background-color: rgba(0,0,0,0.5);
    height: 100vh;
}

.content {
    display: flex;
    align-items: center;
    justify-content: center;
    height: 100%;
    padding: 10px;
}

.card {
    background-color: white;
    min-height: 600px;
    border-radius: 25px;
    padding: 20px;
    display: flex;
    justify-content: center;
    align-items: center;
    width: 100%;
    max-width: 500px;
    box-shadow: rgba(50, 50, 93, 0.25) 0px 50px 100px -20px, rgba(0, 0, 0, 0.3) 0px 30px 60px -30px, rgba(10, 37, 64, 0.35) 0px -2px 6px 0px inset;
}

.card-content {
    display: flex;
    flex-direction: column;
    width: 100%;
    position: relative;
    align-items: center;
}

.welcome {
    font-family: 'Brush Script MT', cursive;
    font-size: 50px;
    margin-bottom: 60px;
}

.input-field {
    height: 50px;
    margin-bottom: 20px;
    outline: none;
    border-radius: 10px;
    border: 1px solid #cecece;
    background-repeat: no-repeat;
    background-position: center left 8px;

    padding-left: 50px;
    max-width: 350px;
    width: 90%;
    display: flex;
}

.wrapper-input {
    width: 100%;
    display: flex;
    flex-direction: column;
    align-items: center;
}

.login-forgot {
    justify-content: space-between;
    display: flex;
    max-width: 350px;
    width: 100%;
    align-items: center;
}

.login-btn {
    height: 50px;
    width: 100px;
    border: none;
    border-radius: 25px;
    color: white;
    background-color: #345fed;
    cursor: pointer;
    font-size: 18px;
    transition: background-color 300ms linear;
}

.login-btn:hover {
    background-color: #54cc39;
}

.hr {
    width: 75%;
    border: 1px solid #c3c3c3;
    border-bottom: none;
    margin-top: 60px;
    margin-bottom: 40px;
}

.input-username {
    background-image: url("../assets/user.png");
}
.input-password {
    background-image: url("../assets/key.png");
}

.error {
    color: red;
    height: 50px;
}

@media (max-width: 374px){
    .login-forgot {
        flex-direction: column-reverse;
    }

    .login-btn {
        margin-bottom: 20px;
    }
}
</style>
