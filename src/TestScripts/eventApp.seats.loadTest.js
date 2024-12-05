import http from 'k6/http';

const scenarios = {
	one: {
		executor: 'constant-arrival-rate',
		rate: 1,
		timeUnit: '1s',
		preAllocatedVUs: 1,
		duration: '2m'
	},
	ten: {
		executor: 'constant-arrival-rate',
		rate: 10,
		timeUnit: '1s',
		preAllocatedVUs: 1,
		duration: '2m',
		startTime: '2m'
	},
	one_hundred: {
		executor: 'constant-arrival-rate',
		rate: 100,
		timeUnit: '1s',
		preAllocatedVUs: 10,
		duration: '2m',
		startTime: '4m'
	},
	one_thousand: {
		executor: 'constant-arrival-rate',
		rate: 1000,
		timeUnit: '1s',
		preAllocatedVUs: 10,
		maxVUs: 300,
		duration: '2m',
		startTime: '6m'
	},
	ten_thousand: {
		executor: 'constant-arrival-rate',
		rate: 10000,
		timeUnit: '1s',
		preAllocatedVUs: 10,
		maxVUs: 1000,
		duration: '2m',
		startTime: '8m'
	}
};

const { SCENARIO } = __ENV;
let scenariosOpts;
if (SCENARIO) {
	const selectedScenario = scenarios[SCENARIO];
	selectedScenario.startTime = '0s';
	scenariosOpts = {
		[SCENARIO] : selectedScenario
	};
} else {
	scenariosOpts = scenarios;
}

export let options = {
	insecureSkipTLSVerify: true,
	noConnectionReuse: false,
	scenarios: scenariosOpts
};

const eventsAppUrl = 'http://events:8080/events';

export default function() {
	const eventId = Math.floor(Math.random() * 3) + 1;
	const sectionId = (eventId-1) * 5 + Math.floor(Math.random() * 5) + 1;
	http.get(eventsAppUrl + `/${eventId}/sections/${sectionId}/seats`, { responseCallback: http.expectedStatuses(200) });
}