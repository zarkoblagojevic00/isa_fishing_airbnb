<template>
    <div class="no-results-container">
        <div>
            <div class="hm">
                Hmmm, we didn't find anything for your search terms.
            </div>
            <hr />
            <div class="explanation">
                We're sorry. Your search yielded no significant results.
            </div>
            <div class="explanation">
                Please, try improving your search terms and search again.
            </div>
            <div class="improvement">
                <div class="improve-title">
                    Here's how to improve your search:
                </div>

                <div
                    class="improvement-tip"
                    v-for="(improvement, idx) in searchImprovements"
                    :key="idx"
                >
                    <span class="point">&#9658;</span> {{ improvement }}
                </div>
            </div>

            <div class="explanation"></div>
        </div>
    </div>
</template>

<script>
export default {
    data() {
        return {
            searchImprovements: [
                "Pick less broad date range - search will yield better results if you're more specific about time interval you're interested in",
                "Pick broader price range - we know it's your hard earned cash, but you deserve it. Treat yourself 😉",
                "Check your spelling - typos are the worst, we know... Please check again if you entered correct service name or location",
            ],
        };
    },
};
</script>

<style scoped>
.no-results-container {
    text-align: left;
    margin-top: 1.5em;
}

hr {
    margin-top: 0.25em;
    margin-bottom: 1em;
    height: 1px;
    background-color: #aca;
    outline: none;
    border: 1px solid #aca;
    border-radius: 0.1em;
}

.hm {
    font-size: 1.9rem;
}

.explanation {
    font-size: 1.1rem;
}

.improvement {
    margin-top: 1rem;
}
.improve-title {
    font-size: 1.3rem;
}

.improvement .improvement-tip {
    margin-top: 0.5rem;
}

.point {
    margin-left: 1.5em;
    color: #aca;
}
</style>
