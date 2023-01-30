redirectToCheckout = function (sessionId) {
    var stripe = Stripe("pk_test_51LcuJ0E7H2nhXFWn3jEj72TsA6Fq44OffW1GAQP2rCdmpgAtC4JHr4vG1HQp0LWWHqoVLKOanVotqpD29bVKqeSI00yIflSvIb")
    stripe.redirectToCheckout({ sessionId: sessionId })
}