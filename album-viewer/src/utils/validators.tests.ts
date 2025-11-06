import { describe, it } from 'mocha';
import { expect } from 'chai';
import {validateDate, validateIPV6} from './validators';

describe('Validators', () => {
    describe('validateDate', () => {
        it('should return true for valid dates', () => {
            expect(validateDate('31/12/2020')).to.be.true;
            expect(validateDate('01/01/2021')).to.be.true;
        });

        it('should return false for invalid dates', () => {
            expect(validateDate('31/02/2020')).to.be.false;
            expect(validateDate('01/13/2021')).to.be.false;
        });
    });

    describe('validateIPV6', () => {
        it('should return true for valid IPv6 addresses', () => {
            expect(validateIPV6('2001:0db8:85a3:0000:0000:8a2e:0370:7334')).to.be.true;
            expect(validateIPV6('::1')).to.be.true;
        });

        it('should return false for invalid IPv6 addresses', () => {
            expect(validateIPV6('2001:0db8:85a3:0000:0000:8a2e:0370:7334:')).to.be.false;
            expect(validateIPV6('::g')).to.be.false;
        });
    });
});
